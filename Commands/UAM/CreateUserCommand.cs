using MediatR;

namespace Commands.UAM
{
    public class CreateUserCommand : IRequest
    {
        public CreateUserCommand()
        {
            Roles = new List<string>();
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public DateTime DOB { get; set; }
        public string? Salutation { get; set; }
        public string? CountryName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime UserCreationDate { get; set; }
        public string? DisplayName { get; set; }
        public string? OrganizationTitle { get; set; }
        public string? ItemId { get; set; }
        public IEnumerable<string> Roles { get; set; }

    }
}