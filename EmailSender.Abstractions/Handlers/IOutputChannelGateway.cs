using System.Threading.Tasks;

namespace EmailSender.Abstractions.Handlers
{
    public interface IOutputChannelGateway
    {
        Task Send<TMessage>(string address, TMessage message);
    }
}
