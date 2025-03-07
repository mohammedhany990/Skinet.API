using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<BaseResponse<string>>
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }
}
