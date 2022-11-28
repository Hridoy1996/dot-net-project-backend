
using AutoMapper;
using Commands.Service;
using Contract;
using Domains.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Net;

namespace CommandHandlers.Service
{
    public class AppointmentRequestCommandHandler : IRequestHandler<AppointmentRequestCommand, CommonResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentManager _appointmentManager;
        private readonly ILogger<AppointmentRequestCommandHandler> _logger;

        public AppointmentRequestCommandHandler(
            IMapper mapper,
            IAppointmentManager appointmentManager,
            ILogger<AppointmentRequestCommandHandler> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _appointmentManager = appointmentManager;
        }

        public async Task<CommonResponseModel> Handle(AppointmentRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var service = _mapper.Map<TelemedicineService>(request);
                bool created = await _appointmentManager.PlaceAppointmentAsync(service);

                return new CommonResponseModel { IsSucceed = created, StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in AppointmentRequestCommandHandler \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }

        }
    }
}