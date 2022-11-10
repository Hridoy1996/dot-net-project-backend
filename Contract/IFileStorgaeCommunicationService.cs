
using Commands.Storage;

namespace Contract
{
    public interface IFileStorgaeCommunicationService
    {
        bool UploadFile(FileUploadCommand command);
        void GetFile(FileUploadCommand command);
        Task DeleteFileAsync(string fileId);
    }
}
