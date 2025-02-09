using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Skinet.API.ExtensionMethods;
using Skinet.API.Middlewares;
using Skinet.Core.Entities.Identity;
using Skinet.Repository.Data;
using Skinet.Repository.Identity;
using StackExchange.Redis;
using System.Reflection;
using FluentValidation;
using MediatR;
using Skinet.API.Behaviours;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Errors;

namespace Skinet.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            #region Add Swagger and Api Versioning
            // API Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSwaggerGen(opt =>
            {
               
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                opt.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                opt.AddSecurityRequirement(securityRequirement);

                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1.0" });

            });
           
            #endregion


            builder.Services.AddDbContext<SkinetDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Configure MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies( Assembly.GetExecutingAssembly()));

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            //builder.Services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            //        options.JsonSerializerOptions.WriteIndented = false; // Disable pretty-print
            //        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping; // Prevents escape issues
            //    });

          


            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                var connection = builder.Configuration.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(connection);
            });


            #region Cors
            builder.Services.AddCors(options =>
               {
                   options.AddPolicy("Policy", policy =>
                   {
                       policy.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .WithOrigins("http://localhost:4200");
                   });
                  
                   options.AddPolicy("AllowDynamicLocalhost", builder =>
                   {
                       builder.SetIsOriginAllowed(origin =>
                           {
                               return new Uri(origin).Host == "localhost";
                           })
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                   });
               });
            #endregion
           

            var app = builder.Build();

            #region Update Database
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var dbContext = service.GetRequiredService<SkinetDbContext>();
            var identityDbContext = service.GetRequiredService<AppIdentityDbContext>();
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedDataAsync(dbContext, loggerFactory);

                var userManager = service.GetRequiredService<UserManager<AppUser>>();

                await identityDbContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during database migration.");
            }

            #endregion


                app.UseSwagger();
                app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
           
            if (app.Environment.IsDevelopment())
            {

               
            }
            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseStaticFiles();
            app.UseCors("Policy");
            app.UseCors("AllowDynamicLocalhost");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
