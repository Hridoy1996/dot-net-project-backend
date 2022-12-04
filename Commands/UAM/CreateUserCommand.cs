using MediatR;
using Shared.Models;

namespace Commands.UAM
{
    public class CreateUserCommand : IRequest<CommonResponseModel>
    {
        public CreateUserCommand()
        {
            Roles = new List<string>();
            DocumentIds = new List<string>();
            Specializations = new List<string>();
            HelthIssues = new List<string>();
        }

        public string? Gender { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DOB { get; set; }
        public string? CountryName { get; set; }
        public string? ProfileImageId { get; set; }
        public string? UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public float? HeightInCm { get; set; }
        public string? MaritalStatus { get; set; }
        public float? WeightInKg { get; set; }
        public string? BloodGroup { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessEmail { get; set; }
        public string? NidNumber { get; set; }
        public IEnumerable<string> DocumentIds { get; set; }
        public IEnumerable<string> HelthIssues { get; set; }
        public IEnumerable<string> Specializations { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public FinancialServiceInfoCommand? FinancialInfo { get; set; }
    }

    public class FinancialServiceInfoCommand
    {
        public string? Type { get; set; }
        public BankFinancialServiceInfoCommand? BankFinancialServiceInfo { get; set; }
        public MobileFinancialServiceInfoCommand? MobileFinancialServiceInfo { get; set; }
    }

    public class BankFinancialServiceInfoCommand
    {
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountHolderName { get; set; }
        public long AccountNumber { get; set; }
        public string? RoutingNumber { get; set; }
    }

    public class MobileFinancialServiceInfoCommand
    {
        public string? PhoneNumber { get; set; }
        public string? ServiceProvider { get; set; }
    }
}