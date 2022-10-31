using Commands.Storage;

namespace Contract
{
    public interface IFileStorgaeCommunicationService
    {
        public void UploadFile( FileUploadCommand command);
        public void GetFile(FileUploadCommand command);
    }
}
