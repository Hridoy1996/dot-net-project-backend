using MongoDB.Bson.Serialization.Attributes;
using Shared.DbEntities.Base;

namespace Domains.Entities
{
    [BsonIgnoreExtraElements]
    public class Role : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsActive { get; set; }
        public bool IsShownInUi { get; set; }
    }
}
