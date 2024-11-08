using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user, IEnumerable<string> roles);
    }
}
