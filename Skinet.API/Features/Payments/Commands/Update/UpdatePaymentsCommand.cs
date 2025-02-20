using MediatR;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommand : IRequest<BaseResponse<CartModel>>
    {
        public string BasketId { get; set; }

    }
}
