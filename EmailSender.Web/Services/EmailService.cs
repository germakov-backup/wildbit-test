using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Dto;

namespace EmailSender.Services
{
    internal class EmailService : IEmailService
    {
        private const string SuccessMessage = "OK";
        private readonly IMessagesTable _messagesTable;
        private readonly IConnectionManager _connectionManager;

        public EmailService(IMessagesTable messagesTable,
            IConnectionManager connectionManager)
        {
            _messagesTable = messagesTable;
            _connectionManager = connectionManager;
        }

        public async Task<MessageResponse> Send(Message message)
        {
            var timestamp = DateTime.UtcNow;
            return await Send(message, timestamp);
        }

        public async Task<IEnumerable<MessageResponse>> Send(IEnumerable<Message> messages)
        {
            using var scope = await _connectionManager.BeginTransactionScope();
            var tasks = new List<Task<MessageResponse>>();
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

        private bool ValidateInputMessage(Message message, out MessageResponse response)
        {
            response = null;
            if (!string.IsNullOrEmpty(message.Id))
            {
                response = new MessageResponse(100, "Input message id not allowed");
            }

            if (string.IsNullOrEmpty(message.To) || !MailAddress.TryCreate(message.To, out _))
            {
                response = new MessageResponse(101, "Target address has invalid format");
            }

            if (string.IsNullOrEmpty(message.From) || !MailAddress.TryCreate(message.From, out _))
            {
                response = new MessageResponse(103, "Source address has invalid format");
            }

            // other structure validation here
            return response == null;
        }

        private async Task<MessageResponse> Send(Message message, DateTime timestamp)
        {
            // I've seen in the API docs that recipient validation business logic is performed on web server
            // For the purpose of the exercise, since performance and reliability are the non-functional goals,
            // I'd consider making those validation asynchronous down and moving to processing pipeline.
            // I don't know requirements for the scale/volume of recipients and complexity of business rules.
            // removing this logic from a synchronous response, would give more flexibility for future.
            // for clients convenience event notifications could be exposed in api based on processing pipeline results
            if (!ValidateInputMessage(message, out var errorResponse))
            {
                return errorResponse;
            }

            var result = await _messagesTable.Save(MapMessageEntityInput(message, timestamp, MessageStatus.Pending));
            return new MessageResponse(new MessageHandle(result.ToString(), message.To, timestamp), 0, SuccessMessage);
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
