

namespace Domains.ResponseDataModels
{
    public class AppointmentsListResponse
    {
        public AppointmentsListResponse()
        {
            ApppointmentResponses = new List<ApppointmentResponse>();
        }

        public List<ApppointmentResponse> ApppointmentResponses { get; set; }
        public long TotalCount { get; set; }    
    }

    public class ApppointmentResponse
    {
        public string? Id { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? PatientUserName { get; set; }
        public string? ApplicantUserId { get; set; }
        public string? ServiceType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? ServiceRequestDate { get; set; }
    }
}
