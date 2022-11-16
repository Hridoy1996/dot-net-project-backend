using Domains.Entities;
using Domains.ResponseDataModels;
using Shared.Models;

namespace Contract
{
    public interface IAppointmentManager
    {
        AppointmentsListResponse GetAppointments(string searchKey, string status, string type, string doctorUserId, int page = 1, int size = 10);
        Task<bool> PlaceAppointmentAsync(TelemedicineService user);
    }
}
