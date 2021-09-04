using System;
using System.Threading.Tasks;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class PriorityScoreFilter : IPipelineFilter<MessageClaimCheck, PriorityScoreFilterOptions>
    {
        private readonly IOutputChannelGateway _channelGateway;

        public PriorityScoreFilter(IOutputChannelGateway channelGateway)
        {
            _channelGateway = channelGateway;
        }

        public string FilterName => nameof(PriorityScoreFilter);

        public Task Handle(MessageClaimCheck input, PriorityScoreFilterOptions config)
        {
            // just dummy
            input.PriorityScore += DateTime.Now.Ticks % 10 <= 5
                ? 0
                : 1;

            return _channelGateway.Send(config.NextStep, input);
        }


    }
}
