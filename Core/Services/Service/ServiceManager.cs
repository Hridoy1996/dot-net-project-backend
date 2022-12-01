using AutoMapper;
using Commands.Service;
using Contract;
using Domains.Entities;
using Domains.ResponseDataModels;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Enums;

namespace Infrastructure.Core.Services.Service
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentManager> _logger;
        public AppointmentManager(IMongoTeleMedicineDBContext mongoTeleMedicineDBContext,
            IMapper mapper,
            ILogger<AppointmentManager> logger
            )
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AppointmentDetails?> GetAppointmentDetailsAsync(string appointmentId, string patientId, string doctorId)
        {
            var filter = Builders<TelemedicineService>.Filter.Eq(x => x.ApplicantUserId, patientId);
            filter &= Builders<TelemedicineService>.Filter.Eq(x => x.AssignedDoctorUserId, doctorId);
            filter &= Builders<TelemedicineService>.Filter.Eq(x => x.ItemId, appointmentId);

            var appointment = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                .Find(filter)
                .FirstOrDefaultAsync();

            var appointmentDetails = _mapper.Map<AppointmentDetails>(appointment);

            return appointmentDetails;
        }

        public async Task<AppointmentHistoryResponse> GetAppointmentHistoryAsync(string? currentAppointmentId, string patientId, string loggedInDoctorId, int pageNumber, int pageSize)
        {
            var filter = Builders<TelemedicineService>.Filter.Eq(x => x.ApplicantUserId, patientId);
            filter &= Builders<TelemedicineService>.Filter.Eq(x => x.AssignedDoctorUserId, loggedInDoctorId);
            filter &= Builders<TelemedicineService>.Filter.Ne(x => x.ItemId, currentAppointmentId);

            var totalCount_Task = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                .Find(filter)
                .CountDocumentsAsync();

            var appointments = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                .Find(filter)
                .SortByDescending(x => x.StartDate)
                .Skip(pageSize * pageNumber)
                .Limit(pageSize)
                .ToListAsync();

            var sppointmentDetailsList = new List<AppointmentDetails>();

            foreach (var appointment in appointments)
            {
                var appointmentDetails = _mapper.Map<AppointmentDetails>(appointment);

                sppointmentDetailsList.Add(appointmentDetails);
            }

            return new AppointmentHistoryResponse { TotalCount = await totalCount_Task, AppointmentDetailsList = sppointmentDetailsList };

        }

        public async Task<AppointmentsListResponse> GetAppointmentsAsync(string searchKey, string status, string type, string doctorUserId, string patientId, int page = 1, int size = 10)
        {
            _logger.LogInformation($"In GetAppointments method: searchKey: {searchKey}, status: {status}, type: {type}, doctorUserId: {doctorUserId}");

            var filter = Builders<TelemedicineService>.Filter.Empty;

            if (!string.IsNullOrEmpty(searchKey))
            {
                filter &= Builders<TelemedicineService>.Filter.Regex(x => x.ApplicantDisplayName, new BsonRegularExpression(searchKey, "i"));
            }
            if (!string.IsNullOrEmpty(status))
            {
                filter &= Builders<TelemedicineService>.Filter.Eq(x => x.Status, status);
            }
            if (!string.IsNullOrEmpty(type))
            {
                filter &= Builders<TelemedicineService>.Filter.Eq(x => x.ServiceType, type);
            }
            if (!string.IsNullOrEmpty(doctorUserId))
            {
                filter &= Builders<TelemedicineService>.Filter.Eq(x => x.AssignedDoctorUserId, doctorUserId);
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                filter &= Builders<TelemedicineService>.Filter.Eq(x => x.ApplicantUserId, patientId);
            }

            var totalCount = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                        .Find(filter)
                        .CountDocumentsAsync();

            var apppointments = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                        .Find(filter)
                        .Skip(page * size)
                        .Limit(size)
                        .ToList()
                        .Select(x =>
                            new ApppointmentResponse
                            {
                                PatientUserName = x.ApplicantUserName,
                                ApplicantUserId = x.ApplicantUserId,
                                ApplicantDisplayName = x.ApplicantDisplayName,
                                Id = x.ItemId,
                                EndDate = x.EndDate == default ? String.Empty : x.EndDate.ToShortDateString(),
                                StartDate = x.StartDate == default ? String.Empty : x.StartDate.ToShortDateString(),
                                StartTime = x.StartDate == default ? String.Empty : x.StartDate.ToShortTimeString(),
                                EndTime = x.EndDate == default ? String.Empty : x.EndDate.ToShortTimeString(),
                                ServiceType = x.ServiceType,
                                Status = x.Status,
                                ServiceRequestDate = x.ServiceInitiationDate == default ? String.Empty : x.ServiceInitiationDate.ToShortDateString()
                            })
                        .ToList();

            return new AppointmentsListResponse { ApppointmentResponses = apppointments, TotalCount = await totalCount };
        }

        public async Task<bool> PlaceAppointmentAsync(TelemedicineService service)
        {
            try
            {
                service.ServiceInitiationDate = DateTime.UtcNow;

                if (service.ServiceType == nameof(AppointmentType.Offline))
                {
                    service.StartDate = DateTime.UtcNow;
                    service.EndDate = DateTime.UtcNow.AddDays(1);
                    service.Status = nameof(AppointmentStatus.Pending);
                    service.AssignedDoctorUserId = "97689638-956c-4c09-b3b2-f835f74c9e57"; //TODO:
                    service.Status = nameof(AppointmentStatus.Ongoing);
                }

                await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s").InsertOneAsync(service);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> ResolveAppointmentAsync(string serviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceId))
                {
                    return false;
                }

                var filter = Builders<TelemedicineService>.Filter.Eq(x => x.ItemId, serviceId);
                var updateDefinition = Builders<TelemedicineService>.Update.Set(x => x.Status, nameof(AppointmentStatus.Resolved));

                var updateResult = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                    .UpdateOneAsync(filter, updateDefinition);

                _logger.LogInformation($"In ResolveAppointmentAsync: updateResult: {JsonConvert.SerializeObject(updateResult)}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> SubmitFeedbackAsync(FeedBackSubmissionCommand request)
        {
            try
            {
                var feedback = _mapper.Map<DoctorFeedback>(request);

                feedback.FollowUpDate = DateTime.Now.AddDays(request?.FollowUpAfter ?? default);
                feedback.LastUpdatedBy = feedback.DoctorUserId;

                await _mongoTeleMedicineDBContext.GetCollection<DoctorFeedback>($"{nameof(DoctorFeedback)}s").InsertOneAsync(feedback);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SyncServiceStatusAsync()
        {
            try
            {
                var currentTime = DateTime.UtcNow;

                var filter = Builders<TelemedicineService>.Filter.Ne(x => x.Status, nameof(AppointmentStatus.Resolved));
                filter &= Builders<TelemedicineService>.Filter.Ne(x => x.EndDate, default);
                filter &= Builders<TelemedicineService>.Filter.Lte(x => x.EndDate, currentTime);
                var updateDefinition = Builders<TelemedicineService>.Update.Set(x => x.Status, nameof(AppointmentStatus.Expired));

                var updateResults = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                    .UpdateManyAsync(filter, updateDefinition);

                _logger.LogInformation($"In SyncServiceStatusAsync: updateResult: {JsonConvert.SerializeObject(updateResults)}");


            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in SyncServiceStatusAsync \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);
            }
        }
    }
}
