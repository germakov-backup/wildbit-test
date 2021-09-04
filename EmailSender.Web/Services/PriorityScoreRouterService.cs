using System;
using EmailSender.Abstractions.Handlers;
using EmailSender.Config;
using EmailSender.PipelineHandlers.Config;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.Options;

namespace EmailSender.Services
{
    public class PriorityScoreRouterService : HandlerServiceBase<PriorityScoreRouter, PriorityScoreRouterOptions, MessageClaimCheck, PriorityScoreRouterServiceOptions>
    {
        private readonly IOptions<PriorityScoreRouterServiceOptions> _options;

        public PriorityScoreRouterService(IOptions<PriorityScoreRouterServiceOptions> options, IMiddlewareConnectionManager middlewareConnectionManager, IServiceProvider services) : base(options, middlewareConnectionManager, services)
        {
            _options = options;
        }

        protected override PriorityScoreRouterOptions GetFilterConfig() => _options.Value;
    }
}
