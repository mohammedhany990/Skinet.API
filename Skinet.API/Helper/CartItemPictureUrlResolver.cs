using AutoMapper;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Entities.Cart;

namespace Skinet.API.Helper
{
    public class CartItemPictureUrlResolver : IValueResolver<CartItem, CartItemModel, string>
    {
        private readonly IConfiguration _configuration;

        public CartItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(CartItem source, CartItemModel destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
