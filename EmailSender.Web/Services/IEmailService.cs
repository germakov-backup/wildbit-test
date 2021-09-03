using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSender.Dto;

namespace EmailSender.Services
{
    public interface IEmailService
    {
        public Task<MessageResponse> Send(Message message);

        public Task<IEnumerable<MessageResponse>> Send(IEnumerable<Message> messages);
    }
}
