using CommandHandlers.Sms;
using Commands.Service;
using Contract;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models;
using System;
using System.Net;

namespace CommandHandlers.Service
{
    public class AppointmentResolveCommandHandler : IRequestHandler<AppointmentResolveCommand, CommonResponseModel>
    {
        private readonly ILogger<AppointmentResolveCommandHandler> _logger;
        private readonly IAppointmentManager _appointmentManager;
        public AppointmentResolveCommandHandler(ILogger<AppointmentResolveCommandHandler> logger,
            IAppointmentManager appointmentManager)
        {
            _logger = logger;
            _appointmentManager = appointmentManager;
        }

        public async Task<CommonResponseModel> Handle(AppointmentResolveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"In AppointmentResolveCommandHandler: request {JsonConvert.SerializeObject(request)}");

                var result = await _appointmentManager.ResolveAppointmentAsync(request?.ServiceId ?? "");

                return new CommonResponseModel { IsSucceed = result, StatusCode = (int)HttpStatusCode.OK };
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in AppointmentResolveCommandHandler \nMessage: {ex.Message} \nStackTrace: {ex.StackTrace}", ex);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }
        }
    }
}
