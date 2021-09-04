using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class EmailRejectionService : HandlerServiceBase<EmailRejectionFilter, EmailRejectionFilterOptions, MessageClaimCheck, EmailRejectionServiceOptions>
    {
        private readonly IOptions<EmailRejectionServiceOptions> _options;

        public EmailRejectionService(IOptions<EmailRejectionServiceOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override EmailRejectionFilterOptions GetFilterConfig() => _options.Value;
    }
}
