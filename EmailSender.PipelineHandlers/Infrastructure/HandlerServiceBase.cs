using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailSender.PipelineHandlers.Infrastructure
{
    public abstract class HandlerServiceBase<THandler, TFilterConfig, TInput, TOptions> : BackgroundService
        where THandler: IPipelineFilter<TInput, TFilterConfig>
        where TInput : IMessageMetadata
        where TOptions: class, IPipelineHandlerOptions
    {
        private readonly IOptions<TOptions> _options;
        private readonly IMiddlewareConnectionManager _middlewareConnectionManager;
        private readonly IServiceProvider _services;

        public HandlerServiceBase(IOptions<TOptions> options,
            IMiddlewareConnectionManager middlewareConnectionManager,
            IServiceProvider services)
        {
            _options = options;
            _middlewareConnectionManager = middlewareConnectionManager;
            _services = services;
        }

        protected abstract TFilterConfig GetFilterConfig();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriptions = new List<IDisposable>();
            try
            {
                using var scope = _services.CreateScope();
                var filter = scope.ServiceProvider.GetRequiredService<THandler>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<THandler>>();
                foreach (var channel in _options.Value.InputChannels)
                {
                    var subscription = await _middlewareConnectionManager.SubscribeReceiver<TInput>(channel, m =>
                    {
                        logger.LogDebug("Message at '{channel}' channel", channel);
                        m.ProcessingHistory.Add(filter.FilterName);
                        return filter.Handle(m, GetFilterConfig());
                    }, stoppingToken);
                    subscriptions.Add(subscription);
                }
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Dispose();
                }
            }
        }
    }
}
