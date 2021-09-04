using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    public class PriorityScoreFilterServiceOptions : PriorityScoreFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "PriorityScoreFilterServiceOptions";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] {InputAddress};
    }
}
