using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class CheckRecipientService : HandlerServiceBase<CheckRecipientPipelineFilter, CheckRecipientFilterOptions, MessageClaimCheck, CheckRecipientServiceOptions>
    {
        private readonly IOptions<CheckRecipientServiceOptions> _options;

        public CheckRecipientService(IOptions<CheckRecipientServiceOptions> options,
            IMiddlewareConnectionManager middlewareConnectionManager,
            IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override CheckRecipientFilterOptions GetFilterConfig() => _options.Value;

    }
}
