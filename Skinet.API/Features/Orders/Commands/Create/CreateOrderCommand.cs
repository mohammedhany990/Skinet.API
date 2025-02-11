using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Commands.Create
{
    public class CreateOrderCommand : IRequest<BaseResponse<OrderModel>>
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressModel ShippingAddress { get; set; }
    }
}
