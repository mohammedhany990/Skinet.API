using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetOrdersForUser
{
    public class GetOrdersForUserQuery : IRequest<BaseResponse<List<OrderModel>>>
    {
    }
}
