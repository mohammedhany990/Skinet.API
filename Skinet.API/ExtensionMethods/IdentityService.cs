using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Interfaces;
using Skinet.Repository.Identity;
using Skinet.Service.Implementation;

namespace Skinet.API.ExtensionMethods
{
    public static class IdentityService
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
        {

            Services.AddScoped<IAuthService, AuthService>();

            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;


            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<AppUser>>();


            Services.AddAuthentication(options =>
                 {
                     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 })
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = configuration["JWT:ValidIssuer"],
                         ValidateAudience = true,
                         ValidAudience = configuration["JWT:ValidAudience"],
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                         ClockSkew = TimeSpan.Zero
                     };

                 });

            return Services;
        }
    }
}
