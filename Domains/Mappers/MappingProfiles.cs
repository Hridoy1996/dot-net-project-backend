using AutoMapper;
using Commands.Service;
using Commands.Test;
using Commands.UAM;
using Domains.Entities;
using Domains.ResponseDataModels;

namespace Domains.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<FinancialServiceInfoCommand, FinancialServiceInfo>();
            CreateMap<MobileFinancialServiceInfoCommand, MobileFinancialServiceInfo>();
            CreateMap<BankFinancialServiceInfoCommand, BankFinancialServiceInfo>();
            CreateMap<CreateUserCommand, TelemedicineAppUser>()
               .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.UserId ?? Guid.NewGuid().ToString())
                )
               .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.PhoneNumber)
                );

            CreateMap<TelemedicineAppUser, UserDataResponse>()
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.Id)
                );

            CreateMap<Commands.Service.PatientData, Domains.Entities.PatientData>();
            CreateMap<Commands.Service.SixInOneMonitorData, Domains.Entities.SixInOneMonitorData>();
            CreateMap<Commands.Service.Otoscope, Domains.Entities.Otoscope>();
            CreateMap<Commands.Service.Stethoscope, Domains.Entities.Stethoscope>();
            CreateMap<AppointmentRequestCommand, TelemedicineService>()
                .ForMember(
                    dest => dest.ItemId,
                    opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString())
                )
                .ForMember(
                    dest => dest.PatientDateOfBirth,
                    opt => opt.MapFrom(src => src.ApplicantDateOfBirth)
                );

            CreateMap<Domains.Entities.PatientData, Domains.ResponseDataModels.PatientData>();
            CreateMap<Domains.Entities.SixInOneMonitorData, Domains.ResponseDataModels.SixInOneMonitorData>();
            CreateMap<Domains.Entities.Otoscope, Domains.ResponseDataModels.Otoscope>();
            CreateMap<Domains.Entities.Stethoscope, Domains.ResponseDataModels.Stethoscope>();
            CreateMap<TelemedicineService, AppointmentDetails>()
                  .ForMember(
                    dest => dest.ServiceRequestDate,
                    opt => opt.MapFrom(src => src.ServiceInitiationDate)
                );
            CreateMap<FeatureRoleMapCreationCommand, FeatureRoleMap>()
               .ForMember(
                    dest => dest.ItemId,
                    opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString())
                );

            CreateMap<Commands.Service.PrescribedMedicine, Domains.Entities.PrescribedMedicine>();
            CreateMap<FeedBackSubmissionCommand, DoctorFeedback>()
                  .ForMember(
                    dest => dest.ItemId,
                    opt => opt.MapFrom(src => Guid.NewGuid().ToString())
                ).ForMember(
                    dest => dest.CreateDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow)
                ).ForMember(
                    dest => dest.LastUpdatedBy,
                    opt => opt.MapFrom(src => DateTime.UtcNow)
                )
                ;

            CreateMap<Domains.Entities.PrescribedMedicine, Domains.ResponseDataModels.PrescribedMedicine>();
            CreateMap<DoctorFeedback, FeedbackResponseModel>();

        }
    }
}
