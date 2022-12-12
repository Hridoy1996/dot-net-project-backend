using Commands.Service;
using Contract;
using Domains.ResponseDataModels;
using Infrastructure.Core.HelperService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

                var loggedInUserId = User.FindFirstValue("UserId");
                var loggedInUserRole = DataConversions.GetRoles(loggedInUserRoleString);

                var appointments = new AppointmentsListResponse();

                if (loggedInUserRole.Contains("Doctor"))
                {
                    appointments = await _appointmentManager.GetAppointmentsAsync(searchKey, status, type, loggedInUserId, string.Empty, pageNumber, pageSize);
                }

                if (loggedInUserRole.Contains("Patient"))
                {
                    appointments = await _appointmentManager.GetAppointmentsAsync(searchKey, status, type, string.Empty, loggedInUserId, pageNumber, pageSize);
                }

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
        public async Task<CommonResponseModel> GetAppointmentDetails(string appointmentId, string patientId)
        {
            try
            {
                var loggedInDoctorId = User.FindFirstValue("UserId");

                var appointment = await _appointmentManager.GetAppointmentDetailsAsync(appointmentId, patientId, loggedInDoctorId);

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

        [HttpGet]
        [Authorize]
        public async Task<CommonResponseModel> GetAppointmentHistory(string? currentAppointmentId, string patientId, int pageNumber = 0, int pageSize = 5)
        {
            try
            {
                var loggedInDoctorId = User.FindFirstValue("UserId");

                var appointment = await _appointmentManager.GetAppointmentHistoryAsync(currentAppointmentId, patientId, loggedInDoctorId, pageNumber, pageSize);

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

        [HttpPost]
        [Authorize]
        public async Task<CommonResponseModel> SubmitFeedback([FromBody] FeedBackSubmissionCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrEmpty(command.ApplicantUserId) || string.IsNullOrEmpty(command.ServiceId))
                {
                    return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.BadRequest };
                }

                _logger.LogInformation($"Controller Method SubmitFeedback: FeedBackSubmissionCommand: {JsonConvert.SerializeObject(command)}");

                var loggedInUserRoleString = User.FindFirstValue("Roles");
                var loggedInUserRole = DataConversions.GetRoles(loggedInUserRoleString);

                if (!loggedInUserRole.Contains("Doctor"))
                {
                    return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.Unauthorized };
                }

                command.DoctorUserId = User.FindFirstValue("UserId");
                command.DoctorDisplayName = User.FindFirstValue(ClaimTypes.GivenName);

                return (CommonResponseModel)await _mediator.Send(command);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in SubmitFeedback method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = "Server Error!", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<CommonResponseModel> GetFeedback(string serviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceId))
                {
                    return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.BadRequest };
                }

                var loggedInUserRoleString = User.FindFirstValue("Roles");
                var loggedInUserId = User.FindFirstValue("UserId");
                var loggedInUserRole = DataConversions.GetRoles(loggedInUserRoleString);

                if (!loggedInUserRole.Contains("Patient"))
                {
                    return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.Unauthorized };
                }

                var feedback = await _appointmentManager.GetFeedbackAsync(serviceId, loggedInUserId);

                if (feedback == null)
                {
                    return new CommonResponseModel { IsSucceed = true, StatusCode = (int)HttpStatusCode.NoContent };
                }
                else
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseData = feedback, ResponseMessage = "Data found!", StatusCode = (int)HttpStatusCode.OK };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in GetFeedback method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseMessage = "Server Error!", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }


        [HttpPut]
        [Authorize]
        public async Task<CommonResponseModel> ResolveAppointment([FromBody] AppointmentResolveCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrEmpty(command.ServiceId))
                {
                    return new CommonResponseModel { IsSucceed = true, ResponseMessage = "Invalid payload!", StatusCode = (int)HttpStatusCode.NoContent };
                }

                command.DoctorId = User.FindFirstValue("UserId");

                return (CommonResponseModel)await _mediator.Send(command);

            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in ResolveAppointment method \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, ResponseData = "ServerError", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
