using Commands.Test;

namespace Contract
{
    public interface IBackendScriptService
    {
         Task<bool> SaveFeatureRoleMaps(FeatureRoleMapCreationCommand command);
         Task<bool> ClearCollectionAsync(string databaseName);
    }
}
