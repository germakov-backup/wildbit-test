using System.Data;
using System.Threading.Tasks;

namespace EmailSender.Abstractions.DataAccess
{
    public interface IConnectionManager
    {
        Task<IDbConnection> GetConnection();

        Task<ITransactionScope> BeginTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
    }
}
