using AutoMapper;
using Commands.Test;
using Contract;
using Domains.Entities;

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
    }
}


