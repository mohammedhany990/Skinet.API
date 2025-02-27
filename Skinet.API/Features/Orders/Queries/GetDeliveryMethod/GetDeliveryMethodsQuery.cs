using MediatR;
using Skinet.API.Features.Orders.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetDeliveryMethod
{
    public class GetDeliveryMethodsQuery : IRequest<BaseResponse<List<DeliveryMethodResponse>>>
    {
    }
}
