using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSender.Dto;

namespace EmailSender.Services
{
    public interface IEmailService
    {
        public Task<MessageHandle> Send(Message message);

        public Task<IEnumerable<MessageHandle>> Send(IEnumerable<Message> messages);
    }
}
