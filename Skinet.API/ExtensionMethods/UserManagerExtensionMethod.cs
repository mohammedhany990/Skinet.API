using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities.Identity;
using System.Security.Claims;

namespace Skinet.API.ExtensionMethods
{
    public static class UserManagerExtensionMethod
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> manager,
            ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await manager.Users.Include(A => A.Address)
                .FirstOrDefaultAsync(E => E.NormalizedEmail == email);

            return user;
        }

    }
}
