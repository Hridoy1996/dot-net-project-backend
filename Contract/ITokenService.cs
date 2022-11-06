using Domains.Entities;

namespace Contract
{
    public interface ITokenService
    {
        string CreateTokenAsync(string phoneNumber, string userId, string displayName, List<string> roles);
    }
}
