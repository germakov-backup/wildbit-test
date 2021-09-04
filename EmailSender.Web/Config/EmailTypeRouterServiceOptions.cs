using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    public class EmailTypeRouterServiceOptions : EmailTypeRouterOptions, IPipelineHandlerOptions
    {
        public const string Key = "EmailTypeRouterServiceOptions";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] {InputAddress};
    }
}
