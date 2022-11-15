using Shared.DbEntities.Base;

namespace Domains.Entities
{
    public class TelemedicineService : BaseEntity
    {
        public string? ApplicantUserId { get; set; }
        public string? ApplicantUserName { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? ServiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public string? ApplicantComment { get; set; }
        public string? DoctorFeedbackId { get; set; }
        public DateTime? ServiceInitiationDate { get; set; }
        public string? AssignedDoctorName { get; set; }
        public string? AssignedDoctorUserId { get; set; }
        public List<PatientData>? PatientData { get; set; }
    }

    public class PatientData
    {
        public string? ApplicantDocumentId { get; set; }
        public List<string>? Tags { get; set; }
    }
}
