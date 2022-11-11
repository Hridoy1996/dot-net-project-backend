using MongoDB.Bson.Serialization.Attributes;
using Shared.DbEntities.Base;

namespace Domains.Entities
{
    [BsonIgnoreExtraElements]
    public class TelemedicineFile : BaseEntity
    {
        public string? Name { get; set; } 
    }
}
