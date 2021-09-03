using System;

namespace EmailSender.Abstractions.Exceptions
{
    public class EmailSenderValidationException : EmailSenderExceptionBase
    {
        public EmailSenderValidationException(int code, string message) : base(code, message)
        {
        }

        public EmailSenderValidationException(int code, string message, Exception innerException) : base(code, message, innerException)
        {
        }
    }
}
