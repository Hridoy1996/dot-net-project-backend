using Commands.SMS;

namespace Contract
{
    public interface IOtpService
    {
        string GenerateHashedOtp(OtpRequestCommand command);
        string GenerateHashedOtp(OtpVerificationCommand command);
    }
}
