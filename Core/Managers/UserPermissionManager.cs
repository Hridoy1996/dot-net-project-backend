using Contract;
using Domains.Entities;
using Domains.ResponseDataModels;
using MongoDB.Driver;

namespace Infrastructure.Core.Managers
{
    public class UserPermissionManager : IUserPermissionManager
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;

        public UserPermissionManager(IMongoTeleMedicineDBContext mongoTeleMedicineDBContext)
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
        }

        public List<UserFeatureRolePermissions> GetUserFeatureRolePermissions(List<string> roles)
        {
            var filter = Builders<FeatureRoleMap>.Filter.In(x => x.RoleName, roles);

            var result =  _mongoTeleMedicineDBContext.GetCollection<FeatureRoleMap>($"{nameof(FeatureRoleMap)}s")
                        .Find(filter)
                        .ToList()
                        .Select(x =>
                            new UserFeatureRolePermissions
                            {
                                AppName = x.AppName,
                                FeatureId = x.FeatureId,
                                FeatureName = x.FeatureName
                            })
                        .ToList();

            return result;            

        }
    }
}
