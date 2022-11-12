using Commands.UAM;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries.UAM;
using Shared.Models;
using System.Net;

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
        public async Task<CommonResponseModel> RegisterUserAsync([FromBody] CreateUserCommand command)
        {
            try
            {
                return (CommonResponseModel)await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in RegisterUserAsync method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost]
        public async Task<CommonResponseModel> LoginAsync([FromBody] LoginQuery command)
        {
            try
            {
                return (CommonResponseModel) await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in LoginAsync method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}

