using AutoMapper;
using Commands.Storage;
using Contract;
using Domains.Entities;
using MediatR;
namespace CommandHandlers.Storage
{

    public class FileUploadCommandHandler : AsyncRequestHandler<FileUploadCommand>
    {
        private readonly IMapper _mapper;
        private readonly IFileStorgaeCommunicationService _fileStorgaeCommunicationService;

        public FileUploadCommandHandler(
            IMapper mapper,
            IFileStorgaeCommunicationService fileStorgaeCommunicationService)
        {
            _mapper = mapper;
            _fileStorgaeCommunicationService = fileStorgaeCommunicationService;
        }


        protected override async Task Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var user = _mapper.Map<TelemedicineAppUser>(request);

                if (string.IsNullOrEmpty(request?.Base64String))
                {
                    _fileStorgaeCommunicationService.GetFile(request);

                }
                else
                {
                    _fileStorgaeCommunicationService.UploadFile(request);
                }
                return;
            }
            catch (Exception ex)
            {

            }

        }
    }

}
