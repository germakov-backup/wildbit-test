using System;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Config;
using Microsoft.Extensions.Logging;

namespace EmailSender.PipelineHandlers.Handlers
{
    public class CheckRecipientPipelineFilter : IPipelineFilter<MessageClaimCheck, CheckRecipientFilterOptions>
    {
        private readonly IOutputChannelGateway _outputChannel;
        private readonly ILogger<CheckRecipientPipelineFilter> _logger;

        public CheckRecipientPipelineFilter(
            IOutputChannelGateway outputChannel,
            ILogger<CheckRecipientPipelineFilter> logger)
        {
            _outputChannel = outputChannel;
            _logger = logger;
        }

        public string FilterName => nameof(CheckRecipientPipelineFilter);

        public Task Handle(MessageClaimCheck input, CheckRecipientFilterOptions options)
        {
            _logger.LogDebug("CheckSenderPipelineFilter: inputMessage - {id}; to: {to}; from: {from}", input.Id, input.To, input.From);

            return !string.Equals(input.To, Const.BlockedAddress, StringComparison.InvariantCultureIgnoreCase)
                ? _outputChannel.Send(options.NextStep, input)
                : _outputChannel.Send(options.RejectStepAddress, input);
        }
    }
}
