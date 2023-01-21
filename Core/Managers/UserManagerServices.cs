using AutoMapper;
using Contract;
using Domains.Entities;
using Domains.ResponseDataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Core.Managers
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly UserManager<TelemedicineAppUser> _userManager;
        private readonly SignInManager<TelemedicineAppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManagerServices> _logger;
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;

        public UserManagerServices(
             IMongoTeleMedicineDBContext mongoTeleMedicineDBContext,
             UserManager<TelemedicineAppUser> userManager,
             ILogger<UserManagerServices> logger,
             IMapper mapper,
             SignInManager<TelemedicineAppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _mapper = mapper;
        }

        public async Task<UserDataResponse> GetUserAsync(string userName)
        {
            var filter = Builders<TelemedicineAppUser>.Filter.Eq(x => x.UserName, userName);
            var user = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>($"ApplicationUsers").Find(filter).FirstOrDefaultAsync();

            if (user == null) return null;

            return _mapper.Map<UserDataResponse>(user);
        }

        public async Task<bool> Login(string userName, string password = "")
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            return result.Succeeded;
        }

        public async Task<bool> RegisterUserAsync(TelemedicineAppUser appUser, string password = "")
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.UserName == appUser.UserName);

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
