using System;

namespace EmailSender.Abstractions.DataAccess.Models
{
    public class MessageEntity
    {
        public int? Id { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string TextBody { get; set; }

        public string HtmlBody { get; set; }

        public string MessageStream { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime? Created { get; set; }
    }
}
