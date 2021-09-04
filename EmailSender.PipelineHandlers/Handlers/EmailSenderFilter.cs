using System.Data;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using Microsoft.Extensions.Logging;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class EmailSenderFilter : IPipelineFilter<MessageClaimCheck, EmailSenderFilterOptions>
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IMessagesTable _messagesTable;
        private readonly IOutputChannelGateway _channelGateway;
        private readonly ILogger<EmailSenderFilter> _logger;
        public string FilterName => nameof(EmailSenderFilter);

        public EmailSenderFilter(ITransactionManager transactionManager,
            IMessagesTable messagesTable,
            IOutputChannelGateway channelGateway,
            ILogger<EmailSenderFilter> logger)
        {
            _transactionManager = transactionManager;
            _messagesTable = messagesTable;
            _channelGateway = channelGateway;
            _logger = logger;
        }

        public async Task Handle(MessageClaimCheck input, EmailSenderFilterOptions config)
        {
            // trying to achieve idempotency by locking table row for input message and checking it's status
            using var transaction = await _transactionManager.BeginTransactionScope(IsolationLevel.Serializable);
            var message = await _messagesTable.GetMessage(input.Id);

            if (message.Status is MessageStatus.Processing or MessageStatus.Pending)
            {
                _logger.LogInformation("Sending message {id}; to: '{to}'; from: '{from}' subject: '{subject}'; body: '{body}; history: {history}'",
                    message.Id,
                    message.To,
                    message.From,
                    message.Subject,
                    message.HtmlBody,
                    string.Join('-', input.ProcessingHistory));

                await _messagesTable.UpdateStatus(input.Id, MessageStatus.Sent);
                transaction.Commit();

                await _channelGateway.Send(config.NextStep, input);
            }
            else
            {
                _logger.LogWarning("Duplicate message '{id} detected' - ignoring", message.Id);
            }
        }
    }
}
