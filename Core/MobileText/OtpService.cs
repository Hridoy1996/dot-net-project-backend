using Commands.SMS;
using Contract;
using Infrastructure.Core.Hashing;

namespace Infrastructure.Core.SmsService
{
    public class OtpService : IOtpService
    {
        public const short otpLength = 4;
        public OtpService()
        {

        }

        public string GenerateRandomOtp()
        {
            string otp = string.Empty;
            Random random = new Random();

            for (int i = 0; i < otpLength; ++i)
            {
                otp += random.Next(0, 9).ToString();
            }

            return otp;
        }

        public string GenerateHashedOtp(OtpRequestCommand command)
        {
            command.Otp = GenerateRandomOtp();

            var keyValues = command.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .ToDictionary(prop => prop.Name.ToUpper(), prop => prop?.GetValue(command, null)?.ToString());

            return HashingService.GetHashString(keyValues);
        }


        public string GenerateHashedOtp(OtpVerificationCommand command)
        {
            var keyValues = command.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .ToDictionary(prop => prop.Name.ToUpper(), prop => prop?.GetValue(command, null)?.ToString());

            return HashingService.GetHashString(keyValues);

        }
    }
}
