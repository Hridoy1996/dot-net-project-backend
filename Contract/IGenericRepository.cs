using MongoDB.Bson;
using MongoDB.Driver;
using Shared.DbEntities.Base;
using System.Linq.Expressions;

namespace Contract
{
    public interface IGenericRepository
    {
        DeleteResult DeleteMany<T>(Expression<Func<T, bool>> filter, string tenantId = "");
        Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> filter, string tenantId = "");
        Task<List<T>> GetItemsAsync<T>(Expression<Func<T, bool>> filter, string tenantId = "", bool isPlural = true);
        Task UpsertAsync<T>(T entity, string tenantId = "", bool isPlural = true)
            where T : BaseEntity;

        void Upsert<T>(T entity, string tenantId = "", bool isPlural = true)
            where T : BaseEntity;

        void UpsertMany<T>(IEnumerable<T> entities, string tenantId = "", bool isCollectionNamePlural = true)
            where T : BaseEntity;

        Task UpsertManyAsync<T>(IEnumerable<T> entities, string tenantId = "", bool isCollectionNamePlural = true)
            where T : BaseEntity;
        IAggregateFluent<BsonDocument> GetBsonItems(string collectionName, FilterDefinition<BsonDocument> filter, string tenantId = "", bool isPlural = true);
        IQueryable<T> GetQueryableItems<T>(Expression<Func<T, bool>> filter, string tenantId = "",
            bool isPlural = true);

        bool Exists<T>(Expression<Func<T, bool>> dataFilters, string tenantId);

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> dataFilters, string tenantId);   

        List<T> GetItems<T>(Expression<Func<T, bool>> filter, int PageNumber, int PageLimit, string Sort, int Order, string TenantId);

        long GetCount<T>(Expression<Func<T, bool>> filter, string TenantId);

        Task<UpdateResult> UpdatePartialAsync(string id, IDictionary<string, object> entity, string collectionName, string organizationId, string tenantId = "");

        Task<UpdateResult> UpdatePartialAsync(Expression<Func<BsonDocument, bool>> filter, IDictionary<string, object> entity, string collectionName, string tenantId = "");

        Task<UpdateResult> UpdatePartialAsync(FilterDefinition<BsonDocument> datafilter, IDictionary<string, object> entity, string collectionName, string organizationId, string tenantId = "");

        Task<IEnumerable<T>> InsertManyAsync<T>(IEnumerable<T> entities, string tenantId = "");
        BulkWriteResult<BsonDocument> UpdateMany(IEnumerable<BsonDocument> entities, string collectionName, string tenantId = "");

        DeleteResult DeleteMany(BsonDocument filter, string collectionName, string tenantId = "", bool isPlural = true);
    }
}