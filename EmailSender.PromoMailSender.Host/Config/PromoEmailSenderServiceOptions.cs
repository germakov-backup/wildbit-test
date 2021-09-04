using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.PromoMailSender.Host.Config
{
    public class PromoEmailSenderServiceOptions: EmailSenderFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "PromoEmailSenderServiceOptions";

        public string InputAddress { get; set; }

        public ICollection<string> InputChannels => new[] {InputAddress};
    }
}
