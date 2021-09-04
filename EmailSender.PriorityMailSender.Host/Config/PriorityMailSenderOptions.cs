using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.PriorityMailSender.Host.Config
{
    public class PriorityMailSenderOptions: EmailSenderFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "PriorityMailSenderOptions";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] {InputAddress};
    }
}
