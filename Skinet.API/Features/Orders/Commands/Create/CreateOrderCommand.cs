using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Commands.Create
{
    public class CreateOrderCommand : IRequest<BaseResponse<string>>
    {
        //public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressModel ShippingAddress { get; set; }
    }
}
