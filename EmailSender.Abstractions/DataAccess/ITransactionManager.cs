using System.Data;
using System.Threading.Tasks;

namespace EmailSender.Abstractions.DataAccess
{
    public interface ITransactionManager
    {
        Task<ITransactionScope> BeginTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
    }
}