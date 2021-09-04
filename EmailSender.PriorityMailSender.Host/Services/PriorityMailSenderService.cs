using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using EmailSender.PriorityMailSender.Host.Config;
using Microsoft.Extensions.Options;

namespace EmailSender.PriorityMailSender.Host.Services
{
    public class PriorityMailSenderService : HandlerServiceBase<EmailSenderFilter, EmailSenderFilterOptions, MessageClaimCheck, PriorityMailSenderOptions>
    {
        private readonly IOptions<PriorityMailSenderOptions> _options;

        public PriorityMailSenderService(IOptions<PriorityMailSenderOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override EmailSenderFilterOptions GetFilterConfig() => _options.Value;
    }
}
