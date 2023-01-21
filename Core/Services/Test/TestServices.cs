using Amazon.S3.Model;
using Domains.Entities;
using Domains.HelperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Queries;

namespace Infrastructure.Core.Services.Test
{
    public class TestServices
    {
        private readonly ILogger<TestServices> _logger;
        private readonly IMongoDbContext _mongoDbContext;
        private readonly UserManager<TelemedicineAppUser> _userManager;
        public TestServices(ILogger<TestServices> logger, IMongoDbContext mongoDbContext, UserManager<TelemedicineAppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _mongoDbContext = mongoDbContext;
        }

        private IMongoCollection<dynamic> GetCollection(GetDataQuery query)
        {
            return _mongoDbContext.GetCollection<dynamic>(query.CollectionName);
        }

        public TelemedicineAppUser GetUser(string userName)
        {
            var y = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            return y??new TelemedicineAppUser();
        }

        public async Task<DynamicQueryResponseHandler> GetAnyDataAsync(GetDataQuery query)
        {
            var result = new DynamicQueryResponseHandler();
            try
            {
                var collection = GetCollection(query).Aggregate();
                if (!string.IsNullOrEmpty(query.Match))
                {
                    collection = collection.Match(query.Match);
                }
                var data = collection.Skip(query.PageNumber * query.PageSize)
                    .Limit(query.PageSize)
                    .ToList()
                    ;
                return await Task.FromResult(result.HandleQuerySuccess(data, System.Net.HttpStatusCode.OK));

            }
            catch (Exception exception)
            {
                _logger.LogError($"GetAnyDataService -> GetAnyData \n Message: {exception.Message} \n StackTrace: {exception.StackTrace}", exception);

                return result.HandleQueryError(System.Net.HttpStatusCode.InternalServerError, "Exception in GetAnyData");
            }
        }
    }
}
