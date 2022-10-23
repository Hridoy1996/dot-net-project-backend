using AspNetCore.Identity.MongoDbCore.Models;

namespace Domains.Entities
{
    public class TelemedicineAppRole : MongoIdentityRole<string>
    {
        public TelemedicineAppRole() : base()
        {
        }

        public TelemedicineAppRole(string roleName) : base(roleName)
        {
        }
    }
}
