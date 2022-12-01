using Contract;
using Domains.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Queries.UAM;
using Shared.Models;
using System.Net;

namespace QueryHandler
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, CommonResponseModel>
    {
        private IUserManagerServices _userManagerServices;
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginQueryHandler> _logger;
        private readonly IKeyStore _keyStore;

        public LoginQueryHandler(IUserManagerServices userManagerServices,
            ILogger<LoginQueryHandler> logger,
            IMongoTeleMedicineDBContext mongoTeleMedicineDBContext,
            IKeyStore keyStore,
            ITokenService tokenService)
        {
            _userManagerServices = userManagerServices;
            _logger = logger;
            _keyStore = keyStore;
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _tokenService = tokenService;
        }

        public async Task<CommonResponseModel> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Roles?.Contains("Patient") ?? false)
                {
                    var val = await _keyStore.GetValueAsync($"TelemedicinePatientOtp_{request.UserName}");
                  
                    if(!string.IsNullOrEmpty(val))
                    {
                        request.Password = "1qazZAQ!";
                    }
                    else
                    {
                        return new CommonResponseModel { IsSucceed = false, ResponseMessage = "login failed", StatusCode = (int)HttpStatusCode.Unauthorized };
                    }

                }

                var isLoginVerified = await _userManagerServices.Login(request.UserName, request.Password);

                if (isLoginVerified)
                {
                    var filter = Builders<TelemedicineAppUser>.Filter.Eq(x => x.UserName, request.UserName.ToLower());
                    var user = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>($"ApplicationUsers").Find(filter).FirstOrDefaultAsync();

                    var token = _tokenService.CreateToken(user.PhoneNumber, user.Id, $"{user.FirstName} {user.LastName}", user.Roles);

                    return new CommonResponseModel { IsSucceed = true, ResponseData = token, ResponseMessage = "login success", StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = false, ResponseMessage = "login failed", StatusCode = (int)HttpStatusCode.BadRequest };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in LoginQueryHandler \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }
        }
    }
}
