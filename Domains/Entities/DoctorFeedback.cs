using Shared.DbEntities.Base;

namespace Domains.Entities
{
    public class DoctorFeedback : BaseEntity
    {
        public string? ApplicantUserId { get; set; }
        public string? ServiceId { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public List<PrescribedMedicine>? PrescribedMedicines { get; set; }
        public List<string>? PrescribedTests { get; set; }
        public string? AdditionalComment { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string? PdfFileId { get; set; }
        public string? DoctorDisplayName { get; set; }
        public string? DoctorUserId { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientCondition { get; set; }
        public string? EcgCondition { get; set; }
        public string? HeartCondition { get; set; }
        public string? LungConition { get; set; }
        public string? OtoscopeConition { get; set; }
    }

    public class PrescribedMedicine
    {
        public string? Name { get; set; }
        public string? Instruction { get; set; }
    }
}
