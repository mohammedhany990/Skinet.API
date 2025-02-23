using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities.Identity;

namespace Skinet.Repository.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<AppUser>();
            var adminId = Guid.NewGuid().ToString();
            var adminRoleId = Guid.NewGuid().ToString();  // Unique ID for Admin role

            // 1️⃣ Seed "Admin" Role
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = adminId,
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "010000",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin@1"),
                }
            );

            builder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1, // Must have a primary key
                    FirstName = "Admin",
                    LastName = "User",
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    AppUserId = adminId
                }
            );


            // 3️⃣ Assign the "Admin" Role to the User
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminId,
                    RoleId = adminRoleId
                }
            );
            base.OnModelCreating(builder);
        }
    }
}
