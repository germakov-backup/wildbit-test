using System.Data;
using System.Threading.Tasks;

namespace EmailSender.Data
{
    internal interface IConnectionManager
    {
        Task<IDbConnection> GetConnection();
    }
}
