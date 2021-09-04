using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class EmailTypeRouterService : HandlerServiceBase<EmailTypeRouter, EmailTypeRouterOptions, MessageClaimCheck, EmailTypeRouterServiceOptions>
    {
        private readonly IOptions<EmailTypeRouterServiceOptions> _options;

        public EmailTypeRouterService(IOptions<EmailTypeRouterServiceOptions> options,
            IMiddlewareConnectionManager middlewareConnectionManager,
            IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override EmailTypeRouterOptions GetFilterConfig() => _options.Value;
    }
}
