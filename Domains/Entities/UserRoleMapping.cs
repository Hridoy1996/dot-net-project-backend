using Shared.DbEntities.Base;

namespace Domains.Entities
{
    public class UserRoleMapping : BaseEntity
    {
        public string? RoleId { get; set; }
        public string? UserId { get; set; }
    }
}
