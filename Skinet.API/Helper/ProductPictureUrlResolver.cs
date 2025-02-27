using AutoMapper;
using Skinet.API.Features.Products.Responses;
using Skinet.Core.Entities;

namespace Skinet.API.Helper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductResponse, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
