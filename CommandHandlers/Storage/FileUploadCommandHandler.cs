using AutoMapper;
using Commands.Storage;
using Contract;
using MediatR;
using Shared.Models;
using System.Net;

namespace CommandHandlers.Storage
{
    public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, CommonResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IFileManagerService _fileManagerService;
        private readonly IFileStorgaeCommunicationService _fileStorgaeCommunicationService;

        public FileUploadCommandHandler(
            IMapper mapper,
            IFileManagerService fileManagerService,
            IFileStorgaeCommunicationService fileStorgaeCommunicationService)
        {
            _mapper = mapper;
            _fileManagerService = fileManagerService;
            _fileStorgaeCommunicationService = fileStorgaeCommunicationService;
        }

        public async Task<CommonResponseModel> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isUploaded = _fileStorgaeCommunicationService.UploadFile(request);
                
                if (isUploaded)
                {
                    await _fileManagerService.SaveFileAsync(request);

                    return new CommonResponseModel
                    {
                        IsSucceed = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        ResponseMessage = "File Uploaded Successfully!"
                    };
                }
                
                return new CommonResponseModel
                {
                    IsSucceed = false,
                    StatusCode = (int)HttpStatusCode.OK,
                    ResponseMessage = "File failed to Upload!"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponseModel
                {
                    IsSucceed = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ResponseMessage = "File failed to Upload!"
                };
            }
        }
    }
        //protected override async Task Handle(FileUploadCommand request, CancellationToken cancellationToken)
        //{
        //    try
        //    {

        //        if (string.IsNullOrEmpty(request?.Base64))
        //        {
        //            _fileStorgaeCommunicationService.GetFile(request);

        //        }
        //        else
        //        {
        //            _fileStorgaeCommunicationService.UploadFile(request);
        //        }
        //        return;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

    //    public async Task<CommonResponseModel> IRequestHandler<FileUploadCommand, CommonResponseModel>.Handle(FileUploadCommand request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {

    //            if (string.IsNullOrEmpty(request?.Base64))
    //            {
    //                _fileStorgaeCommunicationService.GetFile(request);

    //            }
    //            else
    //            {
    //                _fileStorgaeCommunicationService.UploadFile(request);
    //            }
    //            return new CommonResponseModel();
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //}

}
