
using Commands.Storage;

namespace Contract
{
    public interface IFileStorgaeCommunicationService
    {
        public bool UploadFile(FileUploadCommand command);
        public void GetFile(FileUploadCommand command);
    }
}
