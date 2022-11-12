using AutoMapper;
using Contract;
using Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using Shared.Models;

namespace Infrastructure.Core.Managers
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly UserManager<TelemedicineAppUser> _userManager;
        private readonly SignInManager<TelemedicineAppUser> _signInManager;
        private readonly ILogger<UserManagerServices> _logger;

        public UserManagerServices(
             UserManager<TelemedicineAppUser> userManager,
             ILogger<UserManagerServices> logger,
             SignInManager<TelemedicineAppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(string userName, string password = "") 
        {

            var result = await _signInManager.PasswordSignInAsync(userName, password,false,false);

            return result.Succeeded;
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
            catch (Exception exception)
            {
                _logger.LogError($"Error in RegisterUserAsync \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return false;
            }
        }
    }
}
