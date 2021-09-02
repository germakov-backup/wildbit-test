using Dapper.FluentMap.Mapping;
using EmailSender.Abstractions.DataAccess.Models;

namespace EmailSender.Data.DbMappings
{
    internal class MessageMappings : EntityMap<MessageEntity>
    {
        internal MessageMappings()
        {
            Map(m => m.From).ToColumn("sender", false);
            Map(m => m.To).ToColumn("destination", false);
            Map(m => m.TextBody).ToColumn("text_body", false);
            Map(m => m.HtmlBody).ToColumn("html_body", false);
            Map(m => m.MessageStream).ToColumn("message_stream", false);
        }
    }
}
