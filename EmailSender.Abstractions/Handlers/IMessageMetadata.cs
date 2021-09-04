using System.Collections.Generic;

namespace EmailSender.Abstractions.Handlers
{
    // message payload and metadata should be separated in interface, but for timeliness reasons, I just cut corner and merge with payload
    public interface IMessageMetadata
    {
        public IList<string> ProcessingHistory { get; }
    }
}
