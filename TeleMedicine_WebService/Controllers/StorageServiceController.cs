using Commands.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StorageServiceController : Controller
    {
        private readonly ILogger<StorageServiceController> _logger;
        private readonly IMediator _mediator;

        public StorageServiceController(ILogger<StorageServiceController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CommonResponseModel> UploadFile([FromBody] FileUploadCommand command)
        {
            return (CommonResponseModel) await _mediator.Send(command);
        }

        [HttpGet]
        public ActionResult GetFile([FromBody] FileUploadCommand command)
        {
            _mediator.Send(command);

            return Ok();

        }
    }
}
