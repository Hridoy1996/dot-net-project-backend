using Commands.UAM;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserServiceController : ControllerBase
    {
        private readonly ILogger<UserServiceController> _logger;
        private readonly IMediator _mediator;

        public UserServiceController(ILogger<UserServiceController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public CRM Register([FromBody] CreateUserCommand command)
        {
            try
            {
                _mediator.Send(command);

                return new CRM
                {
                    msg = "success",
                    IsSucceed = true
                };
            }
            catch
            {
                return new CRM
                {
                    msg = "failed",
                    IsSucceed = true
                };
            }
        }

        public class CRM
        {
            public string? msg { get; set; }
            public bool IsSucceed { get; set; }
        }
    }
}

