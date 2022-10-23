using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DbEntities.Base
{
    [BsonIgnoreExtraElements]
    public class BaseEntity
    {
        [BsonId]
        public string ItemId { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
        public string? LastUpdatedBy { get; set; }
        public bool IsMarkedToDelete { get; set; }
    }
}
