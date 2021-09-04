using System;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Dto;

namespace EmailSender.Services
{
    internal class EmailValidationService : IEmailValidationService
    {
        public Task<(int, string)> Validate(Message message)
        {
            if (!string.IsNullOrEmpty(message.Id))
            {
                return Task.FromResult((100, "Input message id not allowed"));
            }

            if (string.IsNullOrEmpty(message.To) || !MailAddress.TryCreate(message.To, out _))
            {
                return Task.FromResult((101, "Target address has invalid format"));
            }

            if (string.IsNullOrEmpty(message.From) || !MailAddress.TryCreate(message.From, out _))
            {
                return Task.FromResult((103, "Source address has invalid format"));
            }

            if (string.Equals(message.From, Const.BlockedAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.FromResult((405, "You tried to send from a sender that has been marked as inactive. Found inactive addresses: example@example.com. " +
                                             "Inactive recipients are ones that have generated a hard bounce, a spam complaint, or a manual suppression."));
            }

            return Task.FromResult((Const.SuccessErrorCode, Const.SuccessResponseMessage));
        }
    }
}
