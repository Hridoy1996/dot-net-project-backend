using AutoMapper;
using Commands.Service;
using Commands.UAM;
using Contract;
using Domains.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Net;

namespace CommandHandlers.Service
{
    public class FeedBackSubmissionCommandHandler : IRequestHandler<FeedBackSubmissionCommand, CommonResponseModel>
    { 
        private readonly IMapper _mapper;
        private readonly IAppointmentManager _appointmentManager;
        private readonly ILogger<FeedBackSubmissionCommandHandler> _logger;

        public FeedBackSubmissionCommandHandler(
            IMapper mapper,
            ILogger<FeedBackSubmissionCommandHandler> logger,
            IAppointmentManager appointmentManager)
        {
            _logger = logger;
            _mapper = mapper;
            _appointmentManager = appointmentManager;
        }

        public async Task<CommonResponseModel> Handle(FeedBackSubmissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentManager.SubmitFeedbackAsync(request);

                return new CommonResponseModel { IsSucceed = result, StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in FeedBackSubmissionCommandHandler \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }

        }
    }
}