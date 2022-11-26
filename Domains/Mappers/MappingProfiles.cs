﻿using AutoMapper;
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
                    opt => opt.MapFrom(src => src.ItemId ?? Guid.NewGuid().ToString())
                )
               .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.PhoneNumber)
                );

            CreateMap<TelemedicineAppUser, UserDataResponse>()
                .ForMember(
                    dest => dest.ItemId,
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

            //  public InfrastructureMappingProfile()
            //{
            //    CreateMap(typeof(HttpResponseMessage), typeof(CommonHttpRequestResponse<>))
            //        .ForMember(nameof(CommonHttpRequestResponse<object>.SuccessResponse), opt => opt.Ignore())
            //        .ForMember(nameof(CommonHttpRequestResponse<object>.FailedResponse), opt => opt.Ignore());

            //    CreateMap(typeof(CommonHttpRequestResponse<>), typeof(CommonHttpRequestResponse<>))
            //        .ForMember(nameof(CommonHttpRequestResponse<object>.IsSuccessStatusCode), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.IsSuccessStatusCode)).GetValue(src, null)))
            //        .ForMember(nameof(CommonHttpRequestResponse<object>.SuccessResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.SuccessResponse)).GetValue(src, null)))
            //        .ForMember(nameof(CommonHttpRequestResponse<object>.FailedResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.FailedResponse)).GetValue(src, null)))
            //        /*.ForAllOtherMembers(opt => opt.Ignore())
            //    ;

            //    CreateMap(typeof(CommonHttpRequestResponse<>), typeof(CommonCommandResponse<>))
            //        .ForMember(nameof(CommonCommandResponse<object>.IsSuccess), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.IsSuccessStatusCode)).GetValue(src, null)))
            //        .ForMember(nameof(CommonCommandResponse<object>.SuccessResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.SuccessResponse)).GetValue(src, null)))
            //        .ForMember(nameof(CommonCommandResponse<object>.FailedResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.FailedResponse)).GetValue(src, null)))
            //        /*.ForAllOtherMembers(opt => opt.Ignore())*/;

            //    CreateMap(typeof(CommonHttpRequestResponse<>), typeof(CommonQueryResponse<>))
            //        .ForMember(nameof(CommonQueryResponse<object>.IsSuccess), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.IsSuccessStatusCode)).GetValue(src, null)))
            //        .ForMember(nameof(CommonQueryResponse<object>.SuccessResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.SuccessResponse)).GetValue(src, null)))
            //        .ForMember(nameof(CommonQueryResponse<object>.FailedResponse), map => map.MapFrom(src => src.GetType().GetProperty(nameof(CommonHttpRequestResponse<object>.FailedResponse)).GetValue(src, null)))
            //        /*.ForAllOtherMembers(opt => opt.Ignore())*/;

            //    CreateMap<Unit, LengthUnit>()
            //        .ConvertUsingEnumMapping(option => option
            //        .MapValue(Unit.Meter, LengthUnit.Meter)
            //        .MapValue(Unit.Centimeter, LengthUnit.Centimeter)
            //        .MapValue(Unit.Kilometer, LengthUnit.Kilometer)
            //        .MapValue(Unit.Feet, LengthUnit.Foot)
            //        .MapByName());

            //    CreateMap<Unit, AngleUnit>()
            //        .ConvertUsingEnumMapping(option => option
            //        .MapValue(Unit.Degree, AngleUnit.Degree)
            //        .MapValue(Unit.Radian, AngleUnit.Radian)
            //        .MapByName());

            //}

        }
    }
}
    