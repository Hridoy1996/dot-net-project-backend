using Commands.SMS;
using Contract;
using Infrastructure.Core.Managers;
using MediatR;
using Shared.Models;
using System.Net;
using XAct.Users;

namespace CommandHandlers.Sms
{
    public class OtpVerificationCommandHandler : IRequestHandler<OtpVerificationCommand, CommonResponseModel>
    {
        private readonly IOtpService _otpService;
        private readonly IKeyStore _keyStore;
        private readonly ISmsService _smsService;
        private readonly IUserManagerServices _userManagerServices;
        public OtpVerificationCommandHandler(
            IOtpService otpService,
            IKeyStore keyStore, 
            UserManagerServices userManagerServices,
            ISmsService smsService)
        {
            _userManagerServices = userManagerServices;
            _otpService = otpService;
            _keyStore = keyStore;
            _smsService = smsService;
        }

        public async Task<CommonResponseModel> Handle(OtpVerificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cachedHashString = await _keyStore.GetValueAsync($"TelemedicineOtp_{request.MobileNumber}");

                var hashedOtp = _otpService.GenerateHashedOtp(request);

                if (cachedHashString == hashedOtp)
                {
                    return new CommonResponseModel { IsSucceed = true, StatusCode = (int)HttpStatusCode.OK, ResponseMessage = "verified" };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.Unauthorized, ResponseMessage = "unverified" };
                }
            }
            catch
            {
                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }
        }
    }


}
