using MediatR;

namespace Commands.SMS
{
    public class OtpRequestCommand : IRequest
    {
        public string? MobileNumber { get; set; }
        public string? Role { get; set; }
        public string? Otp { get; set; }
    }
}
