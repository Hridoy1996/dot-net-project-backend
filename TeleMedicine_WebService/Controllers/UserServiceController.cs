using Commands.UAM;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries.UAM;

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

            return Ok();
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginQuery command)
        {
            _mediator.Send(command);

            return Ok();
        }
    }
}

