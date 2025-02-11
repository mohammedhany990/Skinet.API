using Microsoft.AspNetCore.Identity;
using Skinet.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);

    }
}
