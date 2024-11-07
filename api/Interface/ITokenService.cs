using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}
