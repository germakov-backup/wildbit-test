using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class RegularEmailSenderService : HandlerServiceBase<EmailSenderFilter, EmailSenderFilterOptions, MessageClaimCheck, RegularEmailSenderServiceOptions>
    {
        private readonly IOptions<RegularEmailSenderServiceOptions> _options;

        public RegularEmailSenderService(IOptions<RegularEmailSenderServiceOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override EmailSenderFilterOptions GetFilterConfig() => _options.Value;
    }
}
