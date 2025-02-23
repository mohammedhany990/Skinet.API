using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities.Identity;

namespace Skinet.API.ExtensionMethods
{
    public static class UserManagerExtensionMethod
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> manager,
            string email)
        {

            var user = await manager.Users.Include(A => A.Address)
                .FirstOrDefaultAsync(E => E.NormalizedEmail == email);

            return user;
        }

    }
}
