using MediatR;
using Shared.Models;

namespace Commands.Service
{
    public class AppointmentRequestCommand : IRequest<CommonResponseModel>
    {
        public AppointmentRequestCommand()
        {
            PatientData = new List<PatientData>();
        }

        public string? Id { get; set; }
        public string? ApplicantUserId { get; set; }
        public string? ApplicantUserName { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? ServiceType { get; set; }
        public string? ApplicantComment { get; set; }
        public List<PatientData>? PatientData { get; set; }
    }
    public class PatientData
    {
        public string? ApplicantDocumentId { get; set; }
        public List<string>? Tags { get; set; }
    }
}
