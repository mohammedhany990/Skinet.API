using AutoMapper;
using Skinet.API.Features.Products.Models;
using Skinet.Core.Entities;

namespace Skinet.API.Helper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductModel, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductModel destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
