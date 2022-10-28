using Domains.Entities;
namespace Contract
{
    public interface IUserManagerServices
    {
        Task<bool> Login(string email, string password = "");
        Task<bool> RegisterUserAsync(TelemedicineAppUser user, string password = "");
    }
}