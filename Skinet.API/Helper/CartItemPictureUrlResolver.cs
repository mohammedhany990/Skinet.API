using AutoMapper;
using Skinet.API.Features.Carts.Responses;
using Skinet.Core.Entities.Cart;

namespace Skinet.API.Helper
{
    public class CartItemPictureUrlResolver : IValueResolver<CartItem, CartItemResponse, string>
    {
        private readonly IConfiguration _configuration;

        public CartItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(CartItem source, CartItemResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
