using Commands.Storage;
using Commands.Test;
using Contract;
using Infrastructure.Core.Services.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries;
using Shared.Models;
using System.Net;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMediator _mediator;
        private readonly TestServices _testServices;
        private readonly IBackendScriptService _backendScriptService;

        public TestController(ILogger<TestController> logger, IMediator mediator, TestServices testServices, IBackendScriptService backendScriptService)
        {
            _logger = logger;
            _mediator = mediator;
            _testServices = testServices;
            _backendScriptService = backendScriptService;
        }

        [HttpPost]
        [Authorize]
        public async Task<dynamic> GetData([FromBody] GetDataQuery query)
        {
            var response = await _testServices.GetAnyDataAsync(query);

            return Ok(response);
        }  
        
        [HttpDelete]
        [Authorize]
        public async Task<dynamic> ClearCollectionAsync(string collectionName)
        {
            var response = await _backendScriptService.ClearCollectionAsync(collectionName);

            return Ok(response);
        }
        
        [HttpPost]
        //[Authorize]
        public async Task<CommonResponseModel> SaveFeatureRoleMap([FromBody] FeatureRoleMapCreationCommand command)
        {
            try
            {
                return (CommonResponseModel)await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in SaveFeatureRoleMap method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
