using AutoMapper;
using Contract;
using Domains.Entities;
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
            var user = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == email)?? new TelemedicineAppUser();

            await _signInManager.SignInAsync(user, false);
           // var result = await _signInManager.SignInAsync();
            return true;
        }

        public async Task<bool> RegisterUserAsync(TelemedicineAppUser appUser, string password = "")
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == appUser.NormalizedEmail);

                if (user != null)
                {
                    return false;
                }

                var result = await _userManager.CreateAsync(appUser, password);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
