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
                                EndDate = x.EndDate,
                                StartDate = x.StartDate,
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
                _logger.LogInformation($"In method PlaceAppointmentAsync: TelemedicineService: {JsonConvert.SerializeObject(service)}");

                service.ServiceInitiationDate = DateTime.UtcNow;

                var maika = string.Empty;

                if (service.ServiceType == nameof(AppointmentType.Offline))
                {
                    service.StartDate = DateTime.UtcNow;
                    service.EndDate = DateTime.UtcNow.AddDays(1);
                    service.Status = nameof(AppointmentStatus.Pending);

                    var filter = Builders<TelemedicineAppUser>.Filter.AnyIn(x => x.Roles, new List<string> { nameof(TeleMedicineRoles.Doctor) });
                    filter &= Builders<TelemedicineAppUser>.Filter.In(x => x.AvailabilityStatus, new List<string?> { nameof(AvailabilityStatus.Online), String.Empty, null });
                    filter &= Builders<TelemedicineAppUser>.Filter.AnyIn(x => x.Specializations, new List<string> { nameof(DoctorSpecialization.General) });

                    var projectionDefinition = Builders<TelemedicineAppUser>.Projection
                            .Include(doc => doc.Id)
                            .Include(doc => doc.IsCurrentlyServing)
                            ;

                    var availableGeneralDoctors = _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>("ApplicationUsers")
                        .Find(filter)
                        .Project(projectionDefinition)
                        .ToList()
                        ?.Select(x => new TelemedicineAppUser { Id = x.GetValue("_id").ToString(), IsCurrentlyServing = bool.Parse(x.GetValue("IsCurrentlyServing").ToString()) })
                        ?.ToList()
                        ;

                    if (availableGeneralDoctors == null || !availableGeneralDoctors.Any())
                    {
                        _logger.LogInformation($"In method ResolveAppointmentAsync: no doctor found in database for service {JsonConvert.SerializeObject(service)}");

                        return false;
                    }

                    var currentlyFreeDoctorIds = availableGeneralDoctors.Where(x => x.IsCurrentlyServing == false)?.Select(x => x.Id)?.ToList();

                    if (currentlyFreeDoctorIds != null && currentlyFreeDoctorIds.Any())
                    {
                        var serviceFilter = Builders<TelemedicineService>.Filter.In(x => x.AssignedDoctorUserId, currentlyFreeDoctorIds);

                        var docs = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s").Aggregate()
                                     .Group(y => y.AssignedDoctorUserId,
                                            z => new XYZP
                                            {
                                                count = z.Sum(_ => 1),
                                                Id = z.Key
                                            }
                                     ).ToList();

                        docs.OrderBy(x => x.count);

                        var docIds = docs.Select(x => x.Id).ToList();

                        maika = docIds?.FirstOrDefault() ?? docs.FirstOrDefault()?.Id ?? currentlyFreeDoctorIds.First();
                    }
                    else
                    {
                        var currentlyBusyDoctorIds = availableGeneralDoctors.Where(x => x.IsCurrentlyServing == true)?.Select(x => x.Id)?.ToList();

                        if (currentlyBusyDoctorIds == null || !currentlyBusyDoctorIds.Any())
                        {
                            return false;
                        }

                        var serviceFilter = Builders<TelemedicineService>.Filter.In(x => x.AssignedDoctorUserId, currentlyBusyDoctorIds);

                        var docs = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s").Aggregate()
                                        .Group(y => y.AssignedDoctorUserId,
                                            z => new XYZP
                                            {
                                                count = z.Sum(_ => 1),
                                                Id = z.Key
                                            }
                                        ).ToList(); 

                        docs.OrderBy(x => x.count);

                        var docIds = docs.Select(x => x.Id).ToList();

                        maika = docIds?.FirstOrDefault() ?? docs.FirstOrDefault()?.Id ?? currentlyBusyDoctorIds.First();
                    }
                }

                if (string.IsNullOrEmpty(maika))
                {
                    _logger.LogInformation($"In method ResolveAppointmentAsync: no available doctor found for service {JsonConvert.SerializeObject(service)}");

                    return false;
                }

                service.AssignedDoctorUserId = maika; //TODO:

                var filter2 = Builders<TelemedicineAppUser>.Filter.Eq(x => x.Id, service.AssignedDoctorUserId);
                var updateDefinition = Builders<TelemedicineAppUser>.Update.Set(x => x.IsCurrentlyServing, true);

                var updateResult = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>("ApplicationUsers")
                    .UpdateOneAsync(filter2, updateDefinition);

                service.Status = nameof(AppointmentStatus.Ongoing);

                await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s").InsertOneAsync(service);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method ResolveAppointmentAsync:\nerrors: {JsonConvert.SerializeObject(ex)}");

                return false;
            }
        }

        public async Task<bool> ResolveAppointmentAsync(AppointmentResolveCommand command)
        {
            try
            {
                _logger.LogInformation($"In method ResolveAppointmentAsync: command: {JsonConvert.SerializeObject(command)}");

                if (command == null)
                {
                    return false;
                }

                var filter2 = Builders<TelemedicineService>.Filter.Eq(x => x.Status, nameof(AppointmentStatus.Ongoing));
                filter2 &= Builders<TelemedicineService>.Filter.Eq(x => x.AssignedDoctorUserId, command.DoctorId);

                var numberOfservicesforthedoctor = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                    .CountDocumentsAsync(filter2);

                if (numberOfservicesforthedoctor <= 1)
                {
                    var filter3 = Builders<TelemedicineAppUser>.Filter.Eq(x => x.Id, command.DoctorId);
                    var updateDefinition3 = Builders<TelemedicineAppUser>.Update.Set(x => x.IsCurrentlyServing, false);
                    var updateResult3 = await _mongoTeleMedicineDBContext.GetCollection<TelemedicineAppUser>("ApplicationUsers")
                        .UpdateOneAsync(filter3, updateDefinition3);
                }

                var filter = Builders<TelemedicineService>.Filter.Eq(x => x.ItemId, command.ServiceId);
                var updateDefinition = Builders<TelemedicineService>.Update.Set(x => x.Status, nameof(AppointmentStatus.Resolved));

                var updateResult = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                    .UpdateOneAsync(filter, updateDefinition);
                _logger.LogInformation($"In ResolveAppointmentAsync: updateResult: {JsonConvert.SerializeObject(await updateResult)}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error In method ResolveAppointmentAsync:\nerrors: {JsonConvert.SerializeObject(ex)}");

                return false;
            }
        }

        public async Task<bool> SubmitFeedbackAsync(FeedBackSubmissionCommand request)
        {
            try
            {
                _logger.LogInformation($"In method SubmitFeedbackAsync: FeedBackSubmissionCommand: {JsonConvert.SerializeObject(request)}");

                var feedback = _mapper.Map<DoctorFeedback>(request);

                feedback.FollowUpDate = DateTime.Now.AddDays(request?.FollowUpAfter ?? default);
                feedback.LastUpdatedBy = feedback.DoctorUserId;

                await _mongoTeleMedicineDBContext.GetCollection<DoctorFeedback>($"{nameof(DoctorFeedback)}s").InsertOneAsync(feedback);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error In method SubmitFeedbackAsync:\nerrors: {JsonConvert.SerializeObject(ex)}");
                return false;
            }
        }

        public async Task<FeedbackResponseModel> GetFeedbackAsync(string feedbackId, string patiendUserId)
        {
            try
            {
                _logger.LogInformation($"In method GetFeedbackAsync: feedbackId: {feedbackId} patiendUserId {patiendUserId}");

                var filter = Builders<DoctorFeedback>.Filter.Eq(x => x.ItemId, feedbackId);
                filter &= Builders<DoctorFeedback>.Filter.Eq(x => x.ApplicantUserId, patiendUserId);

                var feedBack = await _mongoTeleMedicineDBContext.GetCollection<DoctorFeedback>($"{nameof(DoctorFeedback)}s")
                    .Find(filter)
                    .FirstOrDefaultAsync()
                    ;

                var feedback = _mapper.Map<FeedbackResponseModel>(feedBack);

                return feedback;
            }
            catch (Exception ex)
            {
                _logger.LogError($"In method GetFeedbackAsync: errors: {JsonConvert.SerializeObject(ex)}");

                return null;
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

    public class XYZP
    {
        public string? Id { get; set; }
        public long count { get; set; }
    }
}
