using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;

namespace EmailSender.Data
{
    internal class MessagesTable : IMessagesTable
    {
        private readonly IConnectionManager _connectionManager;

        public MessagesTable(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public Task<MessageEntity> GetMessage(int id)
        {
            return Execute(c => c.QuerySingleAsync<MessageEntity>($"select * from {Const.MessagesTable} where id = @id", new { id = id }));
        }

        public Task<int> Save(MessageEntity messageEntity)
        {
            return Execute(c => Save(messageEntity, c));
        }

        public Task<IEnumerable<int>> Save(IEnumerable<MessageEntity> messages)
        {
            return Execute(async c =>
            {
                var ids = await Task.WhenAll(messages.Select(m => Save(m, c)));
                return (IEnumerable<int>)ids;
            });
        }

        public Task UpdateStatus(int id, MessageStatus status)
        {
            return Execute(c => c.ExecuteAsync($"Update {Const.MessagesTable} set Status = ?status where id = @id", new
            {
                id = id,
                status = status
            }));
        }

        private async Task<int> Save(MessageEntity messageEntity, IDbConnection connection)
        {
            var ret = await connection.QuerySingleAsync<int>($"insert into {Const.MessagesTable}" +
                                           "(sender, destination, subject, text_body, html_body, status, message_stream, created) " +
                                           "values(@from, @to, @subject, @textBody, @htmlBody, @status::MessageStatus, @messageStream, @created) " +
                                           "RETURNING id", new
                                           {
                                               from = messageEntity.From,
                                               to = messageEntity.To,
                                               subject = messageEntity.Subject,
                                               textBody = messageEntity.TextBody,
                                               htmlBody = messageEntity.HtmlBody,
                                               status = messageEntity.Status.ToString(),
                                               messageStream = messageEntity.MessageStream,
                                               created = messageEntity.Created
                                           });

            return ret;
        }

        private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> action)
        {
            using var connection = await _connectionManager.GetConnection();
            return await action(connection);
        }
    }
}
