using MediatR;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Queries.GetCart
{
    public class GetCartQuery : IRequest<BaseResponse<CartModel>>
    {

    }
}
