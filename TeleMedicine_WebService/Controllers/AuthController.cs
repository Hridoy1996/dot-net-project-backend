using Contract;
using Infrastructure.Core.Services.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        private readonly TestServices _testServices;
        private readonly ITokenService _tokenService;

        public AuthController(ILogger<AuthController> logger, 
            IMediator mediator,
            ITokenService tokenService,
            TestServices testServices)
        {
            _tokenService = tokenService;
            _logger = logger;
            _mediator = mediator;
            _testServices = testServices;
        }

        [HttpPost]
        public  dynamic CreateTokenAsync( )
        {
            var response =  _tokenService.CreateToken("1790208848", "5c9471e9-2bfc-4bad-94a1-2edabcc15ec2", "Tareq Mahmud Hridoy",new List<string> { "doctor"});

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult test()
        {
            return Ok("You're Authorized");
        }

    }
}
