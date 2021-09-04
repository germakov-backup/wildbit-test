using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    public class EmailRejectionServiceOptions : EmailRejectionFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "EmailRejectionServiceOptions";
        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] { InputAddress };
    }
}
