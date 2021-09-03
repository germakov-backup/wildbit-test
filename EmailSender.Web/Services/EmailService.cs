using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Dto;

namespace EmailSender.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IMessagesTable _messagesTable;
        private readonly IConnectionManager _connectionManager;

        public EmailService(IMessagesTable messagesTable,
            IConnectionManager connectionManager)
        {
            _messagesTable = messagesTable;
            _connectionManager = connectionManager;
        }

        public async Task<MessageHandle> Send(Message message)
        {
            var timestamp = DateTime.UtcNow;
            return await Send(message, timestamp);
        }

        public async Task<IEnumerable<MessageHandle>> Send(IEnumerable<Message> messages)
        {
            using var scope = await _connectionManager.BeginTransactionScope();
            var tasks = new List<Task<MessageHandle>>();
            foreach (var message in messages)
            {
                var timestamp = DateTime.UtcNow;
                var task = Send(message, timestamp);
                tasks.Add(task);
            }
            var result = await Task.WhenAll(tasks);

            scope.Commit();
            return result;
        }

        private async Task<MessageHandle> Send(Message message, DateTime timestamp)
        {
            var result = await _messagesTable.Save(MapMessageEntityInput(message, timestamp, MessageStatus.Pending));
            return new MessageHandle(result.ToString(), message.To, timestamp);
        }

        // if this get messy, mapping could be extracted to separate service, automapper could be introduced.
        // as long as it's simple, I'll just keep inline
        private MessageEntity MapMessageEntityInput(Message message, DateTime timestamp, MessageStatus status)
        {
            return new MessageEntity()
            {
                Created = timestamp,
                From = message.From,
                Status = status,
                Subject = message.Subject,
                TextBody = message.TextBody,
                HtmlBody = message.HtmlBody,
                To = message.To,
                MessageStream = message.MessageStream
            };
        }
    }
}
