using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EmailSender.Abstractions.Handlers;
using Microsoft.Extensions.Configuration;

namespace EmailSender.PipelineHandlers.Infrastructure
{
    internal class MiddlewareConnectionManager : IMiddlewareConnectionManager,  IDisposable
    {
        private const string RabbitConnectionString = "Rabbit";

        private readonly Lazy<IBus> _inputBus;
        private readonly Lazy<IBus> _outputBus;

        public MiddlewareConnectionManager(IConfiguration configuration)
        {
            _inputBus = new Lazy<IBus>(() => RabbitHutch.CreateBus(configuration.GetConnectionString(RabbitConnectionString)));
            _outputBus = new Lazy<IBus>(() => RabbitHutch.CreateBus(configuration.GetConnectionString(RabbitConnectionString)));
        }

        public async Task<IDisposable> SubscribeReceiver<TInput>(string address, Func<TInput, Task> handler, CancellationToken ct = default)
        {
            return await _inputBus.Value.SendReceive.ReceiveAsync(address, handler, ct);
        }

        public IOutputChannelGateway GetChannelGateway()
        {
            return new RabbitChannelGateway(_outputBus.Value);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_inputBus.IsValueCreated)
                {
                    _inputBus.Value.Dispose();
                }

                if (_outputBus.IsValueCreated)
                {
                    _outputBus.Value.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MiddlewareConnectionManager()
        {
            Dispose(false);
        }
    }
}
