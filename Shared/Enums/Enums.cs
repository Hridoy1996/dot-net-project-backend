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
    
    public enum AvailabilityStatus
    {
        InVacation,
        Available
    }


}
