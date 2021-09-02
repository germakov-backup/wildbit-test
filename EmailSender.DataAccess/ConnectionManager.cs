using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EmailSender.Data
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly string _connectionString;

        public ConnectionManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IDbConnection> GetConnection()
        {
            var c = new NpgsqlConnection(_connectionString);
            await c.OpenAsync();
            return c;
        }
    }
}
