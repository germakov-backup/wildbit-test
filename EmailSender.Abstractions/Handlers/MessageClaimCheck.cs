using System.Collections.Generic;

namespace EmailSender.Abstractions.Handlers
{
    public class MessageClaimCheck : IMessageMetadata
    {
        public int Id { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string MessageStream { get; set; }

        public int PriorityScore { get; set; }

        public IList<string> ProcessingHistory { get; } = new List<string>();
    }
}
