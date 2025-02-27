using MediatR;
using Skinet.API.Features.Orders.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetOrdersForUser
{
    public class GetOrdersForUserQuery : IRequest<BaseResponse<List<OrderResponse>>>
    {
    }
}
