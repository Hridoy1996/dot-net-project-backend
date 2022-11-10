using Commands.Storage;
using Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Net;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StorageServiceController : Controller
    {
        private readonly ILogger<StorageServiceController> _logger;
        private readonly IMediator _mediator;
        private readonly IFileStorgaeCommunicationService _fileStorgaeCommunicationService;

        public StorageServiceController(
            ILogger<StorageServiceController> logger, 
            IMediator mediator,
            IFileStorgaeCommunicationService fileStorgaeCommunicationService)
        {
            _logger = logger;
            _mediator = mediator;
            _fileStorgaeCommunicationService = fileStorgaeCommunicationService;
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
        
        [HttpDelete]
        public async Task<CommonResponseModel> DeleteFile(string fileId)
        {
            try
            {
                await _fileStorgaeCommunicationService.DeleteFileAsync(fileId);

                return new CommonResponseModel { IsSucceed = true, ResponseMessage = "Deleted Successfully", StatusCode = (int) HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error DeleteFile methid \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = "server error", StatusCode = (int)HttpStatusCode.InternalServerError };
            }

}
    }
}
