﻿using MediatR;
using Shared.Models;

namespace Commands.Service
{
    public class AppointmentRequestCommand : IRequest<CommonResponseModel>
    {
        public AppointmentRequestCommand()
        {
            PatientFiles = new List<PatientData>();
            SixInOneMonitorData = new SixInOneMonitorData();
            Stethoscope = new Stethoscope();
            Otoscope = new Otoscope();
        }

        public string? Id { get; set; }
        public string? ApplicantUserId { get; set; }
        public string? ApplicantUserName { get; set; }
        public string? ApplicantDisplayName { get; set; }
        public string? ApplicantDateOfBirth { get; set; }
        public string? ServiceType { get; set; }
        public string? ApplicantComment { get; set; }
        public List<PatientData>? PatientFiles { get; set; }
        public SixInOneMonitorData SixInOneMonitorData { get; set; }
        public Stethoscope Stethoscope { get; set; }
        public Otoscope Otoscope { get; set; }
    }
    public class PatientData
    {
        public string? ApplicantDocumentId { get; set; }
        public List<string>? Tags { get; set; }
    }
    
    public class SixInOneMonitorData
    {
        public string? EcgCsvFileName { get; set; }
        public string? EcgCsvFileId { get; set; }
        public string? EcgPdfFileName { get; set; }
        public string? EcgPdfFileId { get; set; }
        public float SpO2 { get; set; }
        public float Temperature { get; set; }
        public float BloodPressureLow { get; set; }
        public float BloodPressureHigh { get; set; }
        public float HeartRate { get; set; }
        public float GlucoseMonitoring { get; set; }
    }

    public class Stethoscope
    {
        public string? HeartSoundFileName { get; set; }
        public string? HeartSoundFileId { get; set; }
        public string? LungSoundFileName { get; set; }
        public string? LungSoundFileId { get; set; }
    }

    public class Otoscope
    {
        public string? OtoscopeVideoFileName { get; set; }
        public string? OtoscopeVideoFileId { get; set; }
    }
}
