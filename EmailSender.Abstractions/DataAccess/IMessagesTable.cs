using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess.Models;

namespace EmailSender.Abstractions.DataAccess
{
    public interface IMessagesTable
    {
        Task<MessageEntity> GetMessage(int id);

        Task<IList<MessageEntity>> QueryMessages(MessageStatus status, int maxCount);

        Task<int> Save(MessageEntity messageEntity);

        Task<IEnumerable<int>> Save(IEnumerable<MessageEntity> message);

        Task UpdateStatus(int id, MessageStatus status);
    }
}
