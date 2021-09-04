using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.DataAccess.Models;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using Microsoft.Extensions.Logging;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class EmailRejectionFilter : IPipelineFilter<MessageClaimCheck, EmailRejectionFilterOptions>
    {
        private readonly IMessagesTable _messagesTable;
        private readonly ILogger<EmailRejectionFilter> _logger;

        public EmailRejectionFilter(IMessagesTable messagesTable,
            ILogger<EmailRejectionFilter> logger)
        {
            _messagesTable = messagesTable;
            _logger = logger;
        }

        public string FilterName => nameof(EmailRejectionFilter);

        public Task Handle(MessageClaimCheck input, EmailRejectionFilterOptions config)
        {
            _logger.LogInformation("Marked '{id}' message as rejected", input.Id);
            return _messagesTable.UpdateStatus(input.Id, MessageStatus.Failed);
        }
    }
}
