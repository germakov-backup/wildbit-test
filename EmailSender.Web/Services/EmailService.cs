using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Dto;

namespace EmailSender.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IMessagesTable _messagesTable;
        private readonly ITransactionManager _transactionManager;
        private readonly IEmailValidationService _validationService;

        public EmailService(IMessagesTable messagesTable,
            ITransactionManager transactionManager,
            IEmailValidationService validationService)
        {
            _messagesTable = messagesTable;
            _transactionManager = transactionManager;
            _validationService = validationService;
        }

        public async Task<MessageResponse> Send(Message message)
        {
            var timestamp = DateTime.UtcNow;
            return await Send(message, timestamp);
        }

        public async Task<IEnumerable<MessageResponse>> Send(IEnumerable<Message> messages)
        {
            using var scope = await _transactionManager.BeginTransactionScope();
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

        private async Task<MessageResponse> Send(Message message, DateTime timestamp)
        {
            // I've seen in the API docs that recipient validation business logic is performed on web server(error 406)
            // For the purpose of the exercise, since performance and reliability are the non-functional goals,
            // I'd consider making those validation asynchronous and moving down to processing pipeline.
            // Since requirements for the scale/volume of recipients and complexity of business rules are not known at this point,
            // removing this logic from a synchronous response, would give more flexibility for future.
            // for clients convenience event notifications could be exposed in api based on processing pipeline results
            var validation = await _validationService.Validate(message);
            if (validation.Code != Const.SuccessErrorCode)
            {
                return new MessageResponse(validation.Code, validation.Message);
            }

            var result = await _messagesTable.Save(MapMessageEntityInput(message, timestamp, MessageStatus.Pending));
            return new MessageResponse(new MessageHandle(result.ToString(), message.To, timestamp), Const.SuccessErrorCode, Const.SuccessResponseMessage);
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
