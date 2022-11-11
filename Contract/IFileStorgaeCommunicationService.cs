
using Commands.Storage;
using Shared.Models;

namespace Contract
{
    public interface IFileStorgaeCommunicationService
    {
        bool UploadFile(FileUploadCommand command);
        Task<string> GetFileAsBase64(string fileId);
        Task DeleteFileAsync(string fileId);
    }
}
