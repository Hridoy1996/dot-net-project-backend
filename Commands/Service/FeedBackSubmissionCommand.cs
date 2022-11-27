using MediatR;
using Shared.Models;

namespace Commands.Service
{
    public class FeedBackSubmissionCommand : IRequest<CommonResponseModel>
    {
        public FeedBackSubmissionCommand()
        {
            PrescribedMedicines = new List<PrescribedMedicine>();
        }

        public string? ApplicantUserId { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public List<PrescribedMedicine>? PrescribedMedicines { get; set; }
        public List<string>? PrescribedTests { get; set; }
        public string? AdditionalComment { get; set; }
        public int? FollowUpAfter { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientCondition { get; set; }
        public string? DoctorUserId { get; set; }
        public string? DoctorDisplayName { get; set; }
    }

    public class PrescribedMedicine
    {
        public string? Name { get; set; }
        public string? Instruction { get; set; }
    }
}
