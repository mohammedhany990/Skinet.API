using AutoMapper;
using Skinet.API.DTOs.Order;
using Skinet.Core.Entities.Order;

namespace Skinet.API.Helper
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.ItemOrdered.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
