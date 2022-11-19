using Commands.Service;
using Contract;
using Infrastructure.Core.HelperService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.Models;
using System.Net;
using System.Security.Claims;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentManager _appointmentManager;
        private readonly IMediator _mediator;

        public AppointmentController(ILogger<AppointmentController> logger,
            IAppointmentManager appointmentManager,
            IMediator mediator)
        {
            _logger = logger;
            _appointmentManager = appointmentManager;
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize]
        public async Task<CommonResponseModel> RequestAServiceAsync([FromBody] AppointmentRequestCommand command)
        {
            try
            {
                return (CommonResponseModel)await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in RequestAServiceAsync method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<CommonResponseModel> GetAppointmentsAsync(string? searchKey, string? status, string? type, int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var loggedInUserRoleString = User.FindFirstValue("Roles");

                var loggedInDoctorId = User.FindFirstValue("UserId");
                var loggedInUserRole = DataConversions.GetRoles(loggedInUserRoleString);

                if (!loggedInUserRole.Contains("Doctor"))
                {
                    loggedInDoctorId = String.Empty;
                }

                var appointments = await _appointmentManager.GetAppointments(searchKey, status, type, loggedInDoctorId, pageNumber, pageSize);

                if (appointments.TotalCount != 0)
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "Data found!", ResponseData = appointments, StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "No data found!", ResponseData = appointments, StatusCode = (int)HttpStatusCode.NoContent };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in GetAppointmentsAsync method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<CommonResponseModel> GetLatestAppointmentDetails(string patientId)
        {
            try
            {
                var loggedInDoctorId = User.FindFirstValue("UserId");

                var appointment = await _appointmentManager.GetLatestAppointmentDetailsAsync(patientId, loggedInDoctorId);

                if (appointment != null)
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "Data found!", ResponseData = appointment, StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "No data found!", StatusCode = (int)HttpStatusCode.NoContent };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in GetLatestAppointmentDetails method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
