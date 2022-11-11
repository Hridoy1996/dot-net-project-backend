using Contract;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.DbEntities.MongoDB;

namespace Infrastructure.Core.Repository
{
    public class MongoTeleMedicineDBContext : IMongoTeleMedicineDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }

        public MongoTeleMedicineDBContext(IOptions<MongoSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
