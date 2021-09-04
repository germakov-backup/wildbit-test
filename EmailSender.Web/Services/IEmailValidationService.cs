using System.Threading.Tasks;
using EmailSender.Dto;

namespace EmailSender.Services
{
    internal interface IEmailValidationService
    {
        Task<(int Code, string Message)> Validate(Message message);
    }
}
