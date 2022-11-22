using AutoMapper;
using Commands.Test;
using Contract;
using Domains.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using XAct.Users;

namespace Infrastructure.Core.Services.Test
{
    public class BackendScriptService : IBackendScriptService
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;
        private readonly IMapper _mapper;

        public BackendScriptService(IMongoTeleMedicineDBContext mongoTeleMedicineDBContext, IMapper mapper)
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _mapper = mapper;

        }

        public async Task<bool> SaveFeatureRoleMaps(FeatureRoleMapCreationCommand command)
        {
            try
            {
                var featureRoleMap = _mapper.Map<FeatureRoleMap>(command);
                await _mongoTeleMedicineDBContext.GetCollection<FeatureRoleMap>($"{nameof(FeatureRoleMap)}s").InsertOneAsync(featureRoleMap);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 
        
        public async Task<bool> ClearCollectionAsync(string databaseName)
        {
            try
            {
                var filter = Builders<dynamic>.Filter.Empty;
                await _mongoTeleMedicineDBContext.GetCollection<dynamic>($"{databaseName}s").DeleteManyAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAUserAsync(string userName)
        {
            try
            {
                var filter = Builders<TelemedicineAppUser>.Filter.Eq(x=>x.UserName, userName);

                await _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>("ApplicationUsers").DeleteOneAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


