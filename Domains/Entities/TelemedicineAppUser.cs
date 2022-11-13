using AspNetCore.Identity.MongoDbCore.Models;
using Conversions.JsonConverters;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDbGenericRepository.Attributes;
using System.Text.Json.Serialization;

namespace Domains.Entities
{
    [BsonIgnoreExtraElements]
    [CollectionName("ApplicationUsers")]
    public class TelemedicineAppUser : MongoIdentityUser<string>
    {
        
        public TelemedicineAppUser() : base()
        {
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HealthIssues = new List<string>();
            Roles = new List<string>();
        }

        public TelemedicineAppUser(string userName, string email) : base(userName, email)
        {
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HealthIssues = new List<string>();
            Roles = new List<string>();
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NidNumber { get; set; }
        public float? HeightInCm { get; set; }
        public float? WeightInKg { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageId { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessEmail { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Occupation { get; set; }
        public IEnumerable<string> HealthIssues { get; set; }
        public string? MaritalStatus { get; set; }
        public string? BloodGroup { get; set; }
        public string? AvailabilityStatus { get; set; }
        public string? CountryCode { get; set; } = "+880";
        public IEnumerable<string> DocumentIds { get; set; }
        public IEnumerable<string> Specializations { get; set; }
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        public FinancialServiceInfo? FinancialInfo { get; set; }
    }
}
