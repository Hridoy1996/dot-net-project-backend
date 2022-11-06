using MediatR;
using Shared.Models;

namespace Commands.Storage
{
    public class FileUploadCommand : IRequest<CommonResponseModel>
    {
        public FileUploadCommand()
        {
            Tags = new List<string>();
        }

        public string? FileName { get; set; }
        public string? Base64 { get; set; }
        public string? FileId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}