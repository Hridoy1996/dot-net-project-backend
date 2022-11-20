
using Commands.Storage;
using Shared.Models;

namespace Contract
{
    public interface IFileStorgaeCommunicationService
    {
        bool UploadFile(FileUploadCommand command);
        Task DeleteFileAsync(string fileId);
    }
}
