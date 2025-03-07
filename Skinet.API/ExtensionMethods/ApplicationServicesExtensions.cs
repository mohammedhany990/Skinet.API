using Microsoft.AspNetCore.Mvc;
using Skinet.API.Errors;
using Skinet.API.Helper;
using Skinet.Core.Interfaces;
using Skinet.Repository.Abstracts;
using Skinet.Repository.Email;
using Skinet.Repository.Implementation;
using Skinet.Repository.Interfaces;
using Skinet.Service.Abstracts;
using Skinet.Service.Implementation;
using Skinet.Service.Interfaces;

namespace Skinet.API.ExtensionMethods
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IProductTypeService), typeof(ProductTypeService));
            services.AddScoped(typeof(IProductBrandService), typeof(ProductBrandService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IFavoriteService), typeof(FavoriteService));
            services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
            services.AddScoped(typeof(ICartService), typeof(CartService));
            services.AddScoped(typeof(ICartRepositoryFactory), typeof(CartRepositoryFactory));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IEmailSettings), typeof(EmailSettings));
            services.AddScoped(typeof(IAuthorizationServices), typeof(AuthorizationServices));


            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));


            services.AddScoped(typeof(IUserService), typeof(UserService));




            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(m => m.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new ValidationErrorResponse(errors));
                };
            });


            return services;
        }
    }
}
