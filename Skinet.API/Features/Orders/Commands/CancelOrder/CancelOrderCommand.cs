using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest<BaseResponse<string>>
    {
        public int OrderId{ get; set; }
    }
}
