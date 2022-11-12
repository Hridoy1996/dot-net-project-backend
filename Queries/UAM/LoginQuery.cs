using MediatR;
using Shared.Models;

namespace Queries.UAM
{
    public class LoginQuery : IRequest<CommonResponseModel>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
