using Commands.Storage;
using Contract;
using Domains.ResponseDataModels;
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
        private readonly IFileManagerService _fileManagerService;
        private readonly IFileStorgaeCommunicationService _fileStorgaeCommunicationService;

        public StorageServiceController(
            ILogger<StorageServiceController> logger, 
            IMediator mediator,
            IFileManagerService fileManagerService,
            IFileStorgaeCommunicationService fileStorgaeCommunicationService)
        {
            _logger = logger;
            _mediator = mediator;
            _fileManagerService = fileManagerService;
            _fileStorgaeCommunicationService = fileStorgaeCommunicationService;
        }

        [HttpPost]
        public async Task<CommonResponseModel> UploadFile([FromBody] FileUploadCommand command)
        {
            return (CommonResponseModel) await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<CommonResponseModel> GetFile(string fileId)
        {
            try
            {
                var fileResponse = await _fileManagerService.GetFileAsync(fileId);

                return new CommonResponseModel
                {
                    IsSucceed = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    ResponseMessage = "file fetched!",
                    ResponseData = fileResponse
                };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in GetFile method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = "server error", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
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
                _logger.LogError($"Error in DeleteFile method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = "server error", StatusCode = (int)HttpStatusCode.InternalServerError };
            }

}
    }
}
