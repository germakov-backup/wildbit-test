namespace EmailSender.Abstractions.DataAccess.Models
{
    public enum MessageStatus
    {
        Pending,
        Processing,
        Sent,
        Failed,
        Bounced
    }
}
