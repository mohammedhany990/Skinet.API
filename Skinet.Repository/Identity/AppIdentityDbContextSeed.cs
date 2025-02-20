using Microsoft.AspNetCore.Identity;
using Skinet.Core.Entities.Identity;

namespace Skinet.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Hany",
                    Email = "mohammed@gmail.com",
                    UserName = "mohammed",
                    Address = new Address
                    {
                        FirstName = "Mohammed",
                        LastName = "Hany",
                        Street = "123",
                        City = "Shebin",
                        State = "Egypt",
                        ZipCode = "12345"
                    }
                };
                await userManager.CreateAsync(user, "Abcd");
            }
        }
    }
}
