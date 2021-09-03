using System;

namespace EmailSender.Abstractions.Exceptions
{
    public abstract class EmailSenderExceptionBase : Exception
    {
        protected EmailSenderExceptionBase(int code, string message) : this(code, message, null)
        {
        }

        protected EmailSenderExceptionBase(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; }
    }
}
