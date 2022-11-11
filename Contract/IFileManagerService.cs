using Commands.Storage;
using Domains.ResponseDataModels;

namespace Contract
{
    public interface IFileManagerService
    {
        Task SaveFileAsync(FileUploadCommand command);
        Task<FileDataResponse> GetFileAsync(string fileId);
    }
}
