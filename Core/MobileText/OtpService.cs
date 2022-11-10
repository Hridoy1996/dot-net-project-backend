using Commands.SMS;
using Contract;
using System.Text;
using XSystem.Security.Cryptography;

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
          
            StringBuilder sha1StringBuilder = new StringBuilder();
          
            foreach (var valuePair in keyValues.OrderBy(key => key.Key))
            {
                if (!string.IsNullOrEmpty(valuePair.Value))
                {
                    sha1StringBuilder.Append($"{valuePair.Key}={valuePair.Value}");
                }
            }

            var hash1 = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(sha1StringBuilder.ToString()));

            return string.Concat(hash1.Select(b => b.ToString("x2")));
        }

        
    }
}
