using MediatR;
using Skinet.API.Features.Orders.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommand : IRequest<BaseResponse<OrderResponse>>
    {
        public string PaymentIntentId { get; set; }
    }
}
