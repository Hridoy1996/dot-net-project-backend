using Commands.SMS;
using Contract;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommandHandlers.Sms
{
    public class OtpRequegstCommandHandler : AsyncRequestHandler<OtpRequestCommand>
    {
        private readonly IOtpService _otpService;
        private readonly IKeyStore _keyStore;
        private readonly ISmsService _smsService;
        private readonly ILogger<OtpRequegstCommandHandler> _logger;

        public OtpRequegstCommandHandler(IOtpService otpService,
            IKeyStore keyStore,
            ILogger<OtpRequegstCommandHandler> logger,
            ISmsService smsService)
        {
            _logger = logger;
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

            string otpMessage = $"Your OTP is {request.Otp}. Don't share it with anybody!";
            await _smsService.SendTextMessageAsync(otpMessage, MakeMobileNumberElevenDigit(request.MobileNumber));

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
