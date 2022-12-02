using Commands.Service;
using Domains.Entities;
using Domains.ResponseDataModels;

namespace Contract
{
    public interface IAppointmentManager
    {
        Task<AppointmentDetails?> GetAppointmentDetailsAsync(string appointmentId, string patientId, string doctorId);
        Task<AppointmentHistoryResponse> GetAppointmentHistoryAsync(string? currentAppointmentId, string patientId, string loggedInDoctorId, int pageNumber, int pageSize);
        Task<AppointmentsListResponse> GetAppointmentsAsync(string searchKey, string status, string type, string doctorUserId, string patientId, int page = 1, int size = 10);
        Task<bool> PlaceAppointmentAsync(TelemedicineService user);
        Task<bool> ResolveAppointmentAsync(string serviceId);
        Task<bool> SubmitFeedbackAsync(FeedBackSubmissionCommand request);
        Task SyncServiceStatusAsync();
        Task<FeedbackResponseModel> GetFeedbackAsync(string feedbackId, string patiendUserId);
    }
}
