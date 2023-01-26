namespace Domains.ResponseDataModels
{
    public class UserDataResponse
    {
        public UserDataResponse()
        {
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HealthIssues = new List<string>();
            Roles = new List<string>();
        }
        public string? Gender { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public DateTime DOB { get; set; }
        public string? CountryName { get; set; }
        public string? ProfileImageId { get; set; }
        public string? OrganizationTitle { get; set; }
        public string? PhoneNumber { get; set; }
        public float? HeightInCm { get; set; }
        public string? MaritalStatus { get; set; }
        public float? WeightInKg { get; set; }
        public string? BloodGroup { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessEmail { get; set; }
        public string? NidNumber { get; set; }
        public IEnumerable<string> DocumentIds { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> HealthIssues { get; set; }
        public IEnumerable<string> Specializations { get; set; }
    }
}
