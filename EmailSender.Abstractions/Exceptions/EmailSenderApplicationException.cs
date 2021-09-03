using System;

namespace EmailSender.Abstractions.Exceptions
{
    public class EmailSenderApplicationException : EmailSenderExceptionBase
    {
        public EmailSenderApplicationException(int code, string message) : base(code, message)
        {
        }

        public EmailSenderApplicationException(int code, string message, Exception innerException) : base(code, message, innerException)
        {
        }
    }
}
