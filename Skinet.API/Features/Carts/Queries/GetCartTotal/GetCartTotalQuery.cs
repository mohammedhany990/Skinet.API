using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Queries.GetCartTotal
{
    public class GetCartTotalQuery : IRequest<BaseResponse<decimal>>
    {
    }
}
