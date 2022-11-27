using MediatR;
using Shared.Models;

namespace Queries.UAM
{
    public class LoginQuery : IRequest<CommonResponseModel>
    {
        public LoginQuery()
        {
            Roles = new List<string>();
        }

        public List<string> Roles { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
