using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class PriorityScoreFilterService : HandlerServiceBase<PriorityScoreFilter, PriorityScoreFilterOptions, MessageClaimCheck, PriorityScoreFilterServiceOptions>
    {
        private readonly IOptions<PriorityScoreFilterServiceOptions> _options;

        public PriorityScoreFilterService(IOptions<PriorityScoreFilterServiceOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override PriorityScoreFilterOptions GetFilterConfig() => _options.Value;
    }
}
