using MediatR;

namespace Commands.Storage
{
    public class FileUploadCommand : IRequest
    {
        public FileUploadCommand()
        {
            Tags = new List<string>();
        }

        public string? FileName { get; set; }
        public string? Base64String { get; set; }
        public string? FileId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}