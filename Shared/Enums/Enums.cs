using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Enums
{
    public enum Gender
    {
        [BsonRepresentation(BsonType.String)]
        Male,
        [BsonRepresentation(BsonType.String)]
        Female,
        [BsonRepresentation(BsonType.String)]
        Other
    } 
    public enum MaritalStatus
    {
        [BsonRepresentation(BsonType.String)]
        Married,
        [BsonRepresentation(BsonType.String)]
        Unmarried,
        [BsonRepresentation(BsonType.String)]
        Divorced
    }  
    
    public enum DoctorSpecialization
    {
        [BsonRepresentation(BsonType.String)]
        General,
        [BsonRepresentation(BsonType.String)]
        Heart,
        [BsonRepresentation(BsonType.String)]
        Lung
    }   
    
    public enum TeleMedicineRoles
    {
        [BsonRepresentation(BsonType.String)]
        Doctor,
        [BsonRepresentation(BsonType.String)]
        Patient,
        [BsonRepresentation(BsonType.String)]
        Admin
    }  

    public enum AvailabilityStatus
    {
        [BsonRepresentation(BsonType.String)]
        Busy,
        [BsonRepresentation(BsonType.String)]
        Online,
        [BsonRepresentation(BsonType.String)]
        OutOfOffice
    }  
    
    public enum FinancialServiceType
    {
        [BsonRepresentation(BsonType.String)]
        BFS,
        [BsonRepresentation(BsonType.String)]
        MFS
    } 
    
    public enum MobileServiceProvider
    {
        [BsonRepresentation(BsonType.String)]
        Bkash,
        [BsonRepresentation(BsonType.String)]
        Nagad
    }

    public enum AppointmentType
    {
        [BsonRepresentation(BsonType.String)]
        Offline,
        [BsonRepresentation(BsonType.String)]
        SelfCheckup,
        [BsonRepresentation(BsonType.String)]
        Online
    } 
    
    public enum AppointmentStatus
    {
        [BsonRepresentation(BsonType.String)]
        Resolved,
        [BsonRepresentation(BsonType.String)]
        Ongoing,
        [BsonRepresentation(BsonType.String)]
        Upcoming,     
        [BsonRepresentation(BsonType.String)]
        Pending,
        [BsonRepresentation(BsonType.String)]
        Expired
    }

    public enum BloodGroup
    {
        OPositive,
        APositive,
        BPositive,
        ABPositive,
        ONegative,
        ANegative,
        BNegative,
        ABNegative,
        Unknown
    }   
}
