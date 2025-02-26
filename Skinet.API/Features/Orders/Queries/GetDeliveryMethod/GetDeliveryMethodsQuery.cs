using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetDeliveryMethod
{
    public class GetDeliveryMethodsQuery : IRequest<BaseResponse<List<DeliveryMethodModel>>>
    {
    }
}
