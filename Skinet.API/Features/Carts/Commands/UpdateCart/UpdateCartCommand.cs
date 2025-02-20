using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Commands.Update
{
    public class UpdateCartCommand : IRequest<BaseResponse<string>>
    {
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public bool IsPaymentConfirmed { get; set; } = false;

    }
}
