using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    public class CheckRecipientServiceOptions : CheckRecipientFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "CheckRecipientService";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] { InputAddress };
    }
}
