using MediatR;
using Skinet.API.Features.Carts.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommand : IRequest<BaseResponse<CartResponse>>
    {

    }
}
