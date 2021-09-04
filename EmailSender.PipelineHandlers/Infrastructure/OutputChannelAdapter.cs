using System.Threading.Tasks;
using EasyNetQ;
using EmailSender.Abstractions.Handlers;

namespace EmailSender.PipelineHandlers.Infrastructure
{
    internal class RabbitChannelGateway : IOutputChannelGateway
    {
        private readonly IBus _bus;

        public RabbitChannelGateway(IBus bus)
        {
            _bus = bus;
        }

        public Task Send<TMessage>(string address, TMessage message)
        {
            return _bus.SendReceive.SendAsync(address, message);
        }
    }
}
