using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetAllOrderStatuses
{
    public class GetAllOrderStatusesQuery : IRequest<BaseResponse<List<string>>>
    {
    }
}
