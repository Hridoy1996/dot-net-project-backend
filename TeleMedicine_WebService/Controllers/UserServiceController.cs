using Commands.UAM;
using Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserManagerServices _userManagerServices;

        public UserServiceController(ILogger<UserServiceController> logger,
            IMediator mediator,
            IUserManagerServices userManagerServices)
        {
            _logger = logger;
            _mediator = mediator;
            _userManagerServices = userManagerServices;
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
                return (CommonResponseModel)await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in LoginAsync method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<CommonResponseModel> GetUser(string userName)
        {
            try
            {
                var userData = await _userManagerServices.GetUserAsync(userName);

                if (userData == null)
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "User Not Found", StatusCode = (int)HttpStatusCode.NoContent };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "User Found", StatusCode = (int)HttpStatusCode.OK, ResponseData = userData };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in GetUser method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseData = "ServerError", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }


    }

}


