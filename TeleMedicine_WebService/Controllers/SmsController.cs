using Commands.SMS;
using Commands.UAM;
using Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries.UAM;
using Shared.Models;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmsController : Controller
    {
        private readonly ILogger<SmsController> _logger;
        private readonly IMediator _mediator;

        public SmsController(ILogger<SmsController> logger, IMediator mediator, IOtpService otpService)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task ProcessOtp([FromBody] OtpRequestCommand command)
        {
             await _mediator.Send(command);
        }
    }
}
 

