namespace EmailSender.Dto
{
    public class MessageResponse : MessageHandle
    {
        public MessageResponse(MessageHandle handle, int errorCode, string message): base(handle)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
