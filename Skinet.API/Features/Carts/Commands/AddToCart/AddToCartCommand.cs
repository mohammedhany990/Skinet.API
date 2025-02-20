using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Commands.Create
{
    public class AddToCartCommand : IRequest<BaseResponse<string>>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
