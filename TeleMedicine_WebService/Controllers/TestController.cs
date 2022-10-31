using Commands.Storage;
using Infrastructure.Core.Services.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMediator _mediator;
        private readonly TestServices _testServices;

        public TestController(ILogger<TestController> logger, IMediator mediator, TestServices testServices)
        {
            _logger = logger;
            _mediator = mediator;
            _testServices = testServices;
        }

        [HttpPost]
        [Authorize]
        public async Task<dynamic> GetData([FromBody] GetDataQuery query)
        {
            var response = await _testServices.GetAnyDataAsync(query);

            return Ok(response);
        }
    }
}
