using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommand : IRequest<BaseResponse<string>>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
