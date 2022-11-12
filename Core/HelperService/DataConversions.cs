using Newtonsoft.Json;

namespace Infrastructure.Core.HelperService
{
    public static class DataConversions
    {
        public static List<string> GetRoles(string rolesString)
        {
            var roles = JsonConvert.DeserializeObject<List<string>>(rolesString);

            return roles;
        }
    }
}
