using System;
using System.Net;
using EmailSender.Abstractions.Exceptions;
using EmailSender.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace EmailSender.Services
{
    public class MailSendApiExceptionHandlerFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;

        public MailSendApiExceptionHandlerFilter(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var (status, code, message) = MapErrorResponse(context);
            context.Result = new ObjectResult(new MessageResponse(code, message)) { StatusCode = (int)status };
            context.ExceptionHandled = true;
        }

        private (HttpStatusCode Status, int errorCode, string errorMessage) MapErrorResponse(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case EmailSenderApplicationException applicationException:
                    return (HttpStatusCode.InternalServerError, applicationException.Code, applicationException.Message);

                case EmailSenderValidationException validationException:
                    return (HttpStatusCode.BadRequest, validationException.Code, validationException.Message);

                default:
                {
                    return (HttpStatusCode.InternalServerError, 500, "Unexpected error. Please try again later");
                }
            }
        }
    }
}
