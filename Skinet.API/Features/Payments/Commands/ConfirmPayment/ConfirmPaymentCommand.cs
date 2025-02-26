using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommand : IRequest<BaseResponse<OrderModel>>
    {
        public string PaymentIntentId { get; set; }
    }
}
