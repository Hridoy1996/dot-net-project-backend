using AutoMapper;
using Contract;
using Domains.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Core.Managers
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly UserManager<TelemedicineAppUser> _userManager;
        private readonly SignInManager<TelemedicineAppUser> _signInManager;

        public UserManagerServices(
             UserManager<TelemedicineAppUser> userManager
             , SignInManager<TelemedicineAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(string email, string password = "") 
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<bool> RegisterUserAsync(TelemedicineAppUser user, string password = "")
        {
            try
            {
                var absence = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == user.NormalizedEmail);

                if (absence != null)
                {
                    return false;
                }

                var result = await _userManager.CreateAsync(user, password);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
