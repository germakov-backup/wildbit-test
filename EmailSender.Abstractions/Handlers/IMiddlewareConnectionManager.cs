using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSender.Abstractions.Handlers
{
    public interface IMiddlewareConnectionManager
    {
        public Task<IDisposable> SubscribeReceiver<TInput>(string address, Func<TInput, Task> handler, CancellationToken ct = default);

        public IOutputChannelGateway GetChannelGateway();
    }
}
