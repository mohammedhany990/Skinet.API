using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Skinet.API.Errors;
using Skinet.API.Helper;
using Skinet.Core.Interfaces;
using Skinet.Repository;
using Skinet.Service;
using Skinet.Service.Implementation;
using Skinet.Service.Interfaces;

namespace Skinet.API.ExtensionMethods
{
    public  static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IProductTypeService), typeof(ProductTypeService));
            services.AddScoped(typeof(IProductBrandService), typeof(ProductBrandService));


            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                 options.InvalidModelStateResponseFactory = (action) =>
                 {
                     var errors = action.ModelState
                         .Where(e => e.Value.Errors.Count() > 0)
                         .SelectMany(e => e.Value.Errors)
                         .Select(m => m.ErrorMessage)
                         .ToList();

                     var response = new ValidationErrorResponse()
                     {
                         Errors = errors
                     };
                     return new BadRequestObjectResult(response);
                 };
                

            });

            return services;
        }
    }
}
