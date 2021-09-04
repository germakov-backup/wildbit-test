using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    public class PriorityScoreRouterServiceOptions: PriorityScoreRouterOptions, IPipelineHandlerOptions
    {
        public const string Key = "PriorityScoreRouterServiceOptions";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] { InputAddress };
    }
}
