using MediatR;
using Shared.Models;

namespace Commands.Service
{
    public class AppointmentResolveCommand : IRequest<CommonResponseModel>
    {
        public string? ServiceId { get; set; }
        public string? DoctorId { get; set; }
    }
}
