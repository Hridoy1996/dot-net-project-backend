using Conversions.JsonConverters;
using MediatR;
using System.Text.Json.Serialization;

namespace Commands.UAM
{

    public class CreateUserCommand : IRequest
    {
        public CreateUserCommand()
        {
            Roles = new List<string>();
            DocumentIds = new List<string>();
            Specializations = new List<string>();
        }

        public string? Gender { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DOB { get; set; }
        public string? CountryName { get; set; }
        public string? ProfileImageId { get; set; }
        public DateTime UserCreationDate { get; set; }
        public string? DisplayName { get; set; }
        public string? OrganizationTitle { get; set; }
        public string? ItemId { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessEmail { get; set; }
        public string? NidNumber { get; set; }
        public IEnumerable<string> DocumentIds { get; set; }
        public IEnumerable<string> Specializations { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public FinancialServiceInfoCommand? FinancialInfo { get; set; }
    }

    public class FinancialServiceInfoCommand
    {
        public string? FinancialService { get; set; }
        public BankFinancialServiceInfoCommand? BankFinancialServiceInfo { get; set; }
        public MobileFinancialServiceInfoCommand? MobileFinancialServiceInfo { get; set; }
    }

    public class BankFinancialServiceInfoCommand
    {
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountHolderName { get; set; }
        public long AccountNumber { get; set; }
        public long RoutingNumber { get; set; }
    }

    public class MobileFinancialServiceInfoCommand
    {
        public int PhoneNumber { get; set; }
        public string? ServiceProvider { get; set; }
    }
}