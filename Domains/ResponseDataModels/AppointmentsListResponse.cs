

namespace Domains.ResponseDataModels
{
    public class AppointmentsListResponse
    {
        public AppointmentsListResponse()
        {
            ApppointmentResponses = new List<ApppointmentResponse>();
        }

        public List<ApppointmentResponse> ApppointmentResponses { get; set; }
        public int TotalCount { get; set; }    
    }

    public class ApppointmentResponse
    {
        public string? Id { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? ServiceType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
    }
}
