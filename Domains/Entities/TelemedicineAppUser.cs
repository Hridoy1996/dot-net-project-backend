using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using Shared.Enums;

namespace Domains.DBModels
{
    [CollectionName("ApplicationUsers")]
    public class TelemedicineAppUser : MongoIdentityUser<string>
    {
        public TelemedicineAppUser() : base()
        {
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HealthIssues = new List<string>();
        }
        
        public TelemedicineAppUser(string userName, string email) : base(userName, email)
        {
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HealthIssues = new List<string>();
        }
      
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nid { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Occupation { get; set; }
        public IEnumerable<string> HealthIssues { get; set; }
        public float HeightInCm { get; set; }
        public float WreightInKg { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; }
        public string? CountryCode { get; set; }
        public IEnumerable<string> DocumentIds { get; set; }
        public IEnumerable<string> Specializations { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DOB { get; set; }
    }


}
