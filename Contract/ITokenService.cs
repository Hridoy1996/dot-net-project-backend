using Domains.Entities;

namespace Contract
{
    public interface ITokenService
    {
        string CreateToken(string phoneNumber, string userId, string displayName, List<string> roles);
    }
}
