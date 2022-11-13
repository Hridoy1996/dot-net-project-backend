using Domains.Entities;
using Domains.ResponseDataModels;

namespace Contract
{
    public interface IUserManagerServices
    {
        Task<UserDataResponse> GetUserAsync(string userName);
        Task<bool> Login(string email, string password = "");
        Task<bool> RegisterUserAsync(TelemedicineAppUser user, string password = "");
    }
}