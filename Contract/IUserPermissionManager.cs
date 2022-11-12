

using Domains.ResponseDataModels;

namespace Contract
{
    public interface IUserPermissionManager
    {
        public List<UserFeatureRolePermissions> GetUserFeatureRolePermissions(List<string> roles); 
    }
}
