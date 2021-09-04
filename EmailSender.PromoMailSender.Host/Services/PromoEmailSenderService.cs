using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using EmailSender.PromoMailSender.Host.Config;
using Microsoft.Extensions.Options;

namespace EmailSender.PromoMailSender.Host.Services
{
    public class PromoEmailSenderService : HandlerServiceBase<EmailSenderFilter, EmailSenderFilterOptions, MessageClaimCheck, PromoEmailSenderServiceOptions>
    {
        private readonly IOptions<PromoEmailSenderServiceOptions> _options;

        public PromoEmailSenderService(IOptions<PromoEmailSenderServiceOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override EmailSenderFilterOptions GetFilterConfig() => _options.Value;
    }
}
