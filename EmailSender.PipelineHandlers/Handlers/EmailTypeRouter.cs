using System;
using System.Threading.Tasks;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class EmailTypeRouter : IPipelineFilter<MessageClaimCheck, EmailTypeRouterOptions>
    {
        private readonly IOutputChannelGateway _channel;
        // this's just for demo to not load stream from DB
        public const string PromotionalStreamName = "promo";

        public EmailTypeRouter(IOutputChannelGateway channel)
        {
            _channel = channel;
        }

        public string FilterName => nameof(EmailTypeRouter);

        public Task Handle(MessageClaimCheck input, EmailTypeRouterOptions config)
        {
            return string.Equals(input.MessageStream, PromotionalStreamName, StringComparison.InvariantCultureIgnoreCase)
                ? _channel.Send(config.PromotionalChannelAddress, input)
                : _channel.Send(config.TransactionChannelAddress, input);
        }
    }
}
