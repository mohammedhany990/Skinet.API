using Microsoft.AspNetCore.Identity;
using Skinet.Core.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Skinet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);

    }
}
