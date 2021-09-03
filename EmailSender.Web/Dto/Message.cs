using EmailSender.Abstractions.DataAccess.Models;

namespace EmailSender.Dto
{
    public class Message
    {
        public string Id { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string TextBody { get; set; }

        public string HtmlBody { get; set; }

        public string MessageStream { get; set; }
    }
}
