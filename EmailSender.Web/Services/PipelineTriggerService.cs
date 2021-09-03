using System;
using System.Threading;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class PipelineTriggerService : BackgroundService
    {
        private readonly IOptions<TasksPollTriggerOptions> _options;
        private readonly IServiceScope _serviceScope;
        private readonly ILogger<PipelineTriggerService> _logger;
        private readonly IMessagesTable _messagesTable;
        private readonly ITransactionManager _transactionManager;

        public PipelineTriggerService(IOptions<TasksPollTriggerOptions> options,
            IServiceProvider services,
            ILogger<PipelineTriggerService> logger)
        {
            _options = options;
            _serviceScope = services.CreateScope();
            _messagesTable = _serviceScope.ServiceProvider.GetRequiredService<IMessagesTable>();
            _transactionManager = _serviceScope.ServiceProvider.GetRequiredService<ITransactionManager>();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("PipelineTriggerService starting.");

            stoppingToken.Register(() => _logger.LogDebug($" PipelineTriggerService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBatch(_options.Value.BatchSize);
                    await Task.Delay(_options.Value.DelaySeconds * 1000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Messages processing trigger failed");
                }
            }

            _logger.LogDebug($"PipelineTriggerService stopped.");
        }

        public override void Dispose()
        {
            base.Dispose();
            _serviceScope.Dispose();
        }

        private async Task ProcessBatch(int valueBatchSize)
        {
            // could be SELECT FOR UPDATE/SKIP LOCKED to scale producers(but not expected to be needed)
            // in this case need to be added to transaction
            var messages = await _messagesTable.QueryMessages(MessageStatus.Pending, valueBatchSize);

            // here would be nice to have a little bit more thinking about optimal way processing if we target really big scale.
            // transaction is added to batch db writes and avoid 1-1 message and txCommit relation.
            // tradeoff here, that if messaging MW fails to accept some of the messages, whole batch will be retried.
            // i plan to add idempotency check/deduplication down the pipeline, and have batches reasonable small, so this approach not expected to cause big issues
            using var scope = await _transactionManager.BeginTransactionScope();
            foreach (var message in messages)
            {
                await _messagesTable.UpdateStatus(message.Id.Value, MessageStatus.Processing);
            }

            scope.Commit();
        }
    }
}
