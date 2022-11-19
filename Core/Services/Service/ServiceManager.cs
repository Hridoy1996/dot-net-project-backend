using AutoMapper;
using Contract;
using Domains.Entities;
using Domains.ResponseDataModels;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Enums;

namespace Infrastructure.Core.Services.Service
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IMongoTeleMedicineDBContext _mongoTeleMedicineDBContext;
        private readonly IMapper _mapper;

        public AppointmentManager(IMongoTeleMedicineDBContext mongoTeleMedicineDBContext, IMapper mapper)
        {
            _mongoTeleMedicineDBContext = mongoTeleMedicineDBContext;
            _mapper = mapper;
        }

        public async Task<AppointmentDetails?> GetLatestAppointmentDetailsAsync(string patientId)
        {
            var filter = Builders<TelemedicineService>.Filter.Ne(x => x.Status, nameof(AppointmentStatus.Resolved));
            filter &= Builders<TelemedicineService>.Filter.Eq(x => x.ApplicantUserId, patientId);

            var apppointmentFulent = _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s")
                .Find(filter);

            var appointment = await apppointmentFulent.SortByDescending(x => x.ServiceInitiationDate).FirstOrDefaultAsync();

            if (appointment == null)
            {
                return null;
            }

            var appointmentDetails = _mapper.Map<AppointmentDetails>(appointment);

            return appointmentDetails;
        }

        public async Task<AppointmentsListResponse> GetAppointments(string searchKey, string status, string type, string doctorUserId, int page = 1, int size = 10)
        {
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
                                ApplicantUserId = x.ApplicantUserId,
                                ApplicantDisplayName = x.ApplicantDisplayName,
                                Id = x.ItemId,
                                EndDate = x.EndDate == default ? String.Empty : x.EndDate.ToShortDateString(),
                                StartDate = x.StartDate == default ? String.Empty : x.StartDate.ToShortDateString(),
                                ServiceType = x.ServiceType,
                                Status = x.Status
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
                    service.Status = nameof(AppointmentStatus.Ongoing);
                }

                service.AssignedDoctorUserId = "97689638-956c-4c09-b3b2-f835f74c9e57";

                await _mongoTeleMedicineDBContext.GetCollection<TelemedicineService>($"{nameof(TelemedicineService)}s").InsertOneAsync(service);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
