using System.Threading.Tasks;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class PriorityScoreRouter : IPipelineFilter<MessageClaimCheck, PriorityScoreRouterOptions>
    {
        private readonly IOutputChannelGateway _channelGateway;
        public string FilterName => nameof(PriorityScoreRouter);

        public PriorityScoreRouter(IOutputChannelGateway channelGateway)
        {
            _channelGateway = channelGateway;
        }

        public Task Handle(MessageClaimCheck input, PriorityScoreRouterOptions config)
        {
            return input.PriorityScore >= config.PriorityThreshold
                ? _channelGateway.Send(config.PriorityAddress, input)
                : _channelGateway.Send(config.RegularAddress, input);
        }
    }
}
