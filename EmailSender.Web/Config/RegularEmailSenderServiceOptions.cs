using System.Collections.Generic;
using EmailSender.PipelineHandlers.Config;

namespace EmailSender.Config
{
    // implement prioritization by scaling out priority senders.
    // regular sender handles both priority and regular sends for demonstration
    // additional handler process will be dedicated for priority queue
    public class RegularEmailSenderServiceOptions : EmailSenderFilterOptions, IPipelineHandlerOptions
    {
        public const string Key = "RegularEmailSenderServiceOptions";

        public string RegularSendQueueAddress { get; set; }

        public string PrioritySendQueueAddress { get; set; }

        public ICollection<string> InputChannels => new[] {RegularSendQueueAddress, PrioritySendQueueAddress};
    }
}
