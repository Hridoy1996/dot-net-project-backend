using Commands.SMS;
using Commands.UAM;
using Contract;
using MediatR;
using Shared.Models;

namespace CommandHandlers.Sms
{
    public class OtpRequegstCommandHandler : AsyncRequestHandler<OtpRequestCommand>
    {
        private readonly IOtpService _otpService;
        private readonly IKeyStore _keyStore;
        private readonly ISmsService _smsService;
        public OtpRequegstCommandHandler(IOtpService otpService, IKeyStore keyStore, ISmsService smsService)
        {
            _otpService = otpService;
            _keyStore = keyStore;
            _smsService = smsService;
        }

        protected override async Task Handle(OtpRequestCommand request, CancellationToken cancellationToken)
        {
            var hashedOtp = _otpService.GenerateHashedOtp(request);

            if (hashedOtp == null)
            {
                return;
            }

            var addOtpKeyTask = _keyStore.AddKeyWithExpiryAsync($"TelemedicineOtp_{request.MobileNumber}", hashedOtp, (1000 * 60 * 2) + (1000 * 30));

            await _smsService.SendTextMessageAsync(request.Otp, request.MobileNumber);

            await addOtpKeyTask;

            return;
        }
    }
}
