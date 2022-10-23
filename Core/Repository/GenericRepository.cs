using Contract;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;
        private readonly ILogger<GenericRepository> _logger;

        public GenericRepository(
            IMongoTeleMedicineDBContext mongoTeleMedicineDBContext,
            ILogger<GenericRepository> logger)
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _logger = logger;
        }

        
        public DeleteResult DeleteMany<T>(Expression<Func<T, bool>> filter, string tenantId = "")
        {
            var type = typeof(T);
            var collectionName = type.Name + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
            var deleteResult = collection.DeleteMany<T>(filter);
            return deleteResult;
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> filter, string tenantId )
        {
            var type = typeof(T);
            var collectionName = type.Name + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
            var deleteResult = await collection.DeleteManyAsync<T>(filter);
            return deleteResult;
        }

        public IAggregateFluent<BsonDocument> GetBsonItems(string collectionName, FilterDefinition<BsonDocument> filter, string tenantId = "", bool isPlural = true)
        {
            if (isPlural)
            {
                collectionName += "s";
            }
                       
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);
            var find = collection.Aggregate(new AggregateOptions { AllowDiskUse = true }).Match(filter);
          
            return find;
        }
   
        public Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, IDictionary<string, object> updates)
        {
            return UpdateAsync(dataFilters, updates);
        }

        public Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, string tenantId,
            IDictionary<string, object> updates)
        {
            return UpdateAsync(dataFilters, tenantId, updates);
        }

        public void UpdateMany<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "")
        {
            UpdateMany(dataFilters, data, collectionName);
        }

        public Task UpdateManyAsync<T>(Expression<Func<T, bool>> dataFilters, IDictionary<string, object> updates)
        {
            return UpdateManyAsync(dataFilters, updates);
        }

        public Task UpdateManyAsync<T>(Expression<Func<T, bool>> dataFilters, string tenantId,
            IDictionary<string, object> updates)
        {
            return UpdateManyAsync(dataFilters, tenantId, updates);
        }

        public async Task<List<T>> GetItemsAsync<T>(Expression<Func<T, bool>> filter, string tenantId = "6429FC51-C9A3-4B46-9E5C-DD4DB5A57867", bool isPlural = true)
        {
            try
            {
                var type = typeof(T);
                var collectionName = isPlural ? type.Name + "s" : type.Name;
                var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
                var find = await collection.FindAsync<T>(filter);
                var data = find.ToList();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Db Repository error: \n Message: {ex.Message} \n StackTrace: {ex.StackTrace}");
            
                return new List<T>();
            }
        }

        public async Task UpsertAsync<T>(T entity, string tenantId = "", bool isPlural = true)
                where T : Shared.DbEntities.Base.BaseEntity
        {
            var type = typeof(T);
            var collectionName = isPlural ? type.Name + "s" : type.Name;
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            if (string.IsNullOrWhiteSpace(entity.ItemId))
            {
                return;
            }

            entity.LastUpdateDate = DateTime.UtcNow;

            var response = await collection.ReplaceOneAsync<T>(x => x.ItemId == entity.ItemId, entity, new ReplaceOptions { IsUpsert = true });
        }

        public void Upsert<T>(T entity, string tenantId = "", bool isPlural = true)
            where T : Shared.DbEntities.Base.BaseEntity
        {
            var type = typeof(T);
            var collectionName = isPlural ? type.Name + "s" : type.Name;
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            if (string.IsNullOrWhiteSpace(entity.ItemId))
            {
                return;
            }

            entity.LastUpdateDate = DateTime.UtcNow;

            var response = collection.ReplaceOne<T>(x => x.ItemId == entity.ItemId,
                entity, new ReplaceOptions { IsUpsert = true });
        }

        public void UpsertMany<T>(IEnumerable<T> entities, string tenantId = "", bool isCollectionNamePlural = true)
            where T : Shared.DbEntities.Base.BaseEntity
        {
            var type = typeof(T);
            var collectionName = isCollectionNamePlural ? type.Name + "s" : type.Name;
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            var updates = new List<WriteModel<T>>();

            foreach (var doc in entities)
            {
                var filter = Builders<T>.Filter.Eq("_id", doc.ItemId);
                doc.LastUpdateDate = DateTime.UtcNow;

                var updateModel = new ReplaceOneModel<T>(filter, doc) { IsUpsert = true };
                updates.Add(updateModel);
            }
            if (updates.Count > 0)
            {
                var response = collection.BulkWrite(updates);
            }
        }
      
        public async Task UpsertManyAsync<T>(IEnumerable<T> entities, string tenantId = "", bool isCollectionNamePlural = true)
            where T : Shared.DbEntities.Base.BaseEntity
        {
            var type = typeof(T);
            var collectionName = isCollectionNamePlural ? type.Name + "s" : type.Name;
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            var updates = new List<WriteModel<T>>();

            foreach (var doc in entities)
            {
                var filter = Builders<T>.Filter.Eq("_id", doc.ItemId);
                doc.LastUpdateDate = DateTime.UtcNow;

                var updateModel = new ReplaceOneModel<T>(filter, doc) { IsUpsert = true };
                updates.Add(updateModel);
            }
            if (updates.Count > 0)
            {
                var response = await collection.BulkWriteAsync(updates);
            }
        }

        public IQueryable<T> GetQueryableItems<T>(Expression<Func<T, bool>> filter, string tenantId = "", bool isPlural = true)
        {
            var type = typeof(T);
            var collectionName = isPlural ? type.Name + "s" : type.Name;
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
            var find = collection.AsQueryable<T>().Where(filter);
          
            return find;
        }

        public bool Exists<T>(Expression<Func<T, bool>> dataFilters, string tenantId)
        {
            var type = typeof(T);
            var collectionName = $"{type.Name}s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
            var find = collection.Find<T>(dataFilters);

            return find.Any();
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> dataFilters, string tenantId)
        {
            var type = typeof(T);
            var collectionName = $"{type.Name}s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);
            var find = await collection.FindAsync<T>(dataFilters);

            return find.Any();
        }

        public List<T> GetItems<T>(Expression<Func<T, bool>> filter, int PageNumber, int PageLimit, string Sort, int Order, string TenantId)
        {
         
            var type = typeof(T);
            var collectionName = $"{type.Name}s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            var find = collection
                .Find<T>(filter)
                .Sort(new BsonDocument(Sort, Order))
                .Skip(PageNumber * PageLimit)
                .Limit(PageLimit);

            return find.ToList();
        }

        public long GetCount<T>(Expression<Func<T, bool>> filter, string TenantId)
        {
            var type = typeof(T);
            var collectionName = $"{type.Name}s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

            return collection.CountDocuments(filter);
        }

        public async Task<UpdateResult> UpdatePartialAsync(string id, IDictionary<string, object> entity, string collectionName, string organizationId, string tenantId = "")
        {
            collectionName = collectionName + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);

            var idfilter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var filter = idfilter;
            UpdateDefinition<BsonDocument>? update = null;

            foreach (var item in entity)
            {
                if (update == null)
                {
                    var builder = Builders<BsonDocument>.Update;
                    update = builder.Set(item.Key, item.Value);
                }
                else
                {
                    update = update.Set(item.Key, item.Value);
                }
            }

            return await collection.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdatePartialAsync(Expression<Func<BsonDocument, bool>> filter, IDictionary<string, object> entity, string collectionName, string tenantId = "")
        {
            collectionName = collectionName + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);

            UpdateDefinition<BsonDocument>? update = null;

            foreach (var item in entity)
            {
                if (update == null)
                {
                    var builder = Builders<BsonDocument>.Update;
                    update = builder.Set(item.Key, item.Value);
                }
                else
                {
                    update = update.Set(item.Key, item.Value);
                }
            }

            return await collection.UpdateManyAsync(filter, update);
        }

        public async Task<UpdateResult> UpdatePartialAsync(FilterDefinition<BsonDocument> datafilter, IDictionary<string, object> entity, string collectionName, string organizationId, string tenantId = "")
        {
            collectionName = collectionName + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);
            var filter = datafilter;
            UpdateDefinition<BsonDocument>? update = null;

            foreach (var item in entity)
            {
                if (update == null)
                {
                    var builder = Builders<BsonDocument>.Update;
                    update = builder.Set(item.Key, item.Value);
                }
                else
                {
                    update = update.Set(item.Key, item.Value);
                }
            }

            return await collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<T>> InsertManyAsync<T>(IEnumerable<T> entities, string tenantId = "")
        {
            if (entities.Any())
            {
                var type = typeof(T);
                var collectionName = $"{type.Name}s";
                var collection = _mongoTeleMedicineDBContext.GetCollection<T>(collectionName);

                await collection.InsertManyAsync(entities);
            }

            return entities;
        }

        public BulkWriteResult<BsonDocument> UpdateMany(IEnumerable<BsonDocument> entities, string collectionName, string tenantId = "")
        {
            collectionName = $"{collectionName}s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);

            var updates = new List<WriteModel<BsonDocument>>();

            foreach (var doc in entities)
            {
                var filter = new BsonDocument { { "_id", doc.GetValue("_id") } };
                var replace = new ReplaceOneModel<BsonDocument>(filter, doc);
                replace.IsUpsert = true;
                updates.Add(replace);
            }

            return collection.BulkWrite(updates);
        }

        public DeleteResult DeleteMany(BsonDocument filter, string collectionName, string tenantId = "", bool isPlural = true)
        {
            collectionName = collectionName + "s";
            var collection = _mongoTeleMedicineDBContext.GetCollection<BsonDocument>(collectionName);

            return collection.DeleteMany(filter);
        }
    }

}
