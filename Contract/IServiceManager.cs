using Domains.Entities;
using Domains.ResponseDataModels;

namespace Contract
{
    public interface IAppointmentManager
    {
        Task<AppointmentDetails?> GetLatestAppointmentDetailsAsync();
        Task<AppointmentsListResponse> GetAppointments(string searchKey, string status, string type, string doctorUserId, int page = 1, int size = 10);
        Task<bool> PlaceAppointmentAsync(TelemedicineService user);
    }
}
