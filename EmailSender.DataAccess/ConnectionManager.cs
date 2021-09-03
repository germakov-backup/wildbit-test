using System.Data;
using System.Threading;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EmailSender.Data
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly string _connectionString;
        private readonly AsyncLocal<SimpleTransactionScope> _currentTransaction = new();

        public ConnectionManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IDbConnection> GetConnection()
        {
            if (_currentTransaction.Value is {IsDisposed: false })
            {
                return _currentTransaction.Value.Transaction.Connection;
            }

            var c = new NpgsqlConnection(_connectionString);
            await c.OpenAsync();
            return c;
        }

        public async Task<ITransactionScope> BeginTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            if (_currentTransaction.Value is { IsDisposed: false })
            {
                return _currentTransaction.Value;
            }

            var c = new NpgsqlConnection(_connectionString);
            await c.OpenAsync();
            var tx = await c.BeginTransactionAsync(level);
            _currentTransaction.Value = new SimpleTransactionScope(tx);

            return _currentTransaction.Value;
        }
    }
}
