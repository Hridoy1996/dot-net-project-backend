using Commands.SMS;
using Contract;
using MediatR;

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

            var addOtpKeyTask = _keyStore.AddKeyWithExpiryAsync($"TelemedicineOtp_{request.MobileNumber}", hashedOtp, 1000 * 60 * 5);

            await _smsService.SendTextMessageAsync(request.Otp, MakeMobileNumberElevenDigit(request.MobileNumber));

            await addOtpKeyTask;

            return;
        }

        private string MakeMobileNumberElevenDigit(string? mobileNumber)
        {
            if (mobileNumber?.Length == 10)
            {
                return $"0{mobileNumber}";
            }
            else if (mobileNumber?.Length == 11)
            {
                return mobileNumber;
            }

            return string.Empty;
        }
    }
}
