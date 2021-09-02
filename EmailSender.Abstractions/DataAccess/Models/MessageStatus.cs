namespace EmailSender.Abstractions.DataAccess.Models
{
    public enum MessageStatus
    {
        Pending,
        Processing,
        Sending,
        Sent,
        Failed,
        Bounced
    }
}
