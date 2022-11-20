using Commands.Storage;
using Contract;
using Domains.Entities;
using Domains.ResponseDataModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Core.Services.Storage
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;

        public FileManagerService(IMongoTeleMedicineDBContext mongoTeleMedicineDBContext)
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
        }

        public async Task SaveFileAsync(FileUploadCommand command)
        {
            var file = new TelemedicineFile
            {
                ItemId = command?.FileId ?? Guid.NewGuid().ToString(),
                CreateDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                Name = command?.FileName
            };

            await _mongoTeleMedicineDBContext.GetCollection<TelemedicineFile>($"{nameof(TelemedicineFile)}s").InsertOneAsync(file);
        }
        
        public async Task<FileDataResponse> GetFileAsync(string fileId)
        {
            var filter = Builders<TelemedicineFile>.Filter.Eq(x=>x.ItemId, fileId);

            var result = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineFile>($"{nameof(TelemedicineFile)}s")
                        .Find(filter)
                        .FirstOrDefaultAsync();

            if(result is not null)
            {
                var fileAndExtension = GetFileEntensiosizen(result.Name);

                return new FileDataResponse 
                { 
                    FileName = fileAndExtension.Key, 
                    Extension = fileAndExtension.Value,
                    Url = $"https://dot-net-app-space.fra1.digitaloceanspaces.com/telemedicine/test/{fileId}"
                };
            }

            return null;
        }

        KeyValuePair<string, string> GetFileEntensiosizen(string fileName)
        {
            var x = fileName.Split(".").ToList();

            int size = x.Count();

            string fileNameWithOutExtension = string.Empty;
            
            for (int i = 0; i < size - 1; i++)
            {
                fileNameWithOutExtension += x[i];
            }

            return new KeyValuePair<string, string>(fileNameWithOutExtension, x[size-1]);
        }
    }
}
