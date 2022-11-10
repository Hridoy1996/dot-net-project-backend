using Commands.SMS;
using Commands.UAM;
using Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries.UAM;
using Shared.Models;
using System.Net;

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
        
        [HttpPost]
        public async Task<CommonResponseModel> VerifyOtp([FromBody] OtpVerificationCommand command)
        {
            if(command == null || 
               command?.Otp?.Length != 4 ||
               command?.MobileNumber?.Length < 10 ||
               command?.MobileNumber?.Length > 11 ||
               string.IsNullOrEmpty(command?.Role))
            {
                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.BadGateway, ResponseMessage = "invalid payload" };
            }

            return (CommonResponseModel) await _mediator.Send(command);
        }
    }
}
 

