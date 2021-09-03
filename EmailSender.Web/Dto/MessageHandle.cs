using System;

namespace EmailSender.Dto
{
    public class MessageHandle
    {
        public MessageHandle(MessageHandle handle): this(handle?.MessageID, handle?.To, handle?.SubmittedAt)
        {
        }

        public MessageHandle(string messageId, string to, DateTime? submittedAt)
        {
            MessageID = messageId;
            To = to;
            SubmittedAt = submittedAt;
        }

        public string To { get; set; }
        public DateTime? SubmittedAt { get; set; }

        public string MessageID { get; set; }
    }
}
