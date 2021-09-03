using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSender.Dto;
using EmailSender.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(MailSendApiExceptionHandlerFilter))]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Email([FromBody] Message message)
        {
            var id = await _emailService.Send(message);
            return Ok(id);
        }

        [HttpPost("emails")]
        public async Task<IActionResult> Email([FromBody] IEnumerable<Message> message)
        {
            var id = await _emailService.Send(message);
            return Ok(id);
        }
    }
}
