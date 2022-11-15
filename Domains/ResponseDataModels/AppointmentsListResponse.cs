

namespace Domains.ResponseDataModels
{
    public class AppointmentsListResponse
    {
        public string? Id { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? ServiceType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
    }
}
