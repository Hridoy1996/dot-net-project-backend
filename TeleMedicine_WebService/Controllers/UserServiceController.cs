using Commands.UAM;
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
        public ActionResult Register([FromBody] CreateUserCommand command)
        {
            _mediator.Send(command);

            return Ok();

        }
    }
}

