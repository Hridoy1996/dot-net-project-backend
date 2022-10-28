using Domains.Entities;

namespace Contract
{
    public interface ITokenService
    {
        string CreateTokenAsync(TelemedicineAppUser appicationUser);
    }
}
