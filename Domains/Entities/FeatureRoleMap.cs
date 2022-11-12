using MongoDB.Bson.Serialization.Attributes;
using Shared.DbEntities.Base;

namespace Domains.Entities
{
    [BsonIgnoreExtraElements]
    public class FeatureRoleMap : BaseEntity
    {
        public string? AppType { get; set; }
        public string? AppName { get; set; }
        public string? FeatureId { get; set; }
        public string? FeatureName { get; set; }
        public string? RoleName { get; set; }
    }
}
