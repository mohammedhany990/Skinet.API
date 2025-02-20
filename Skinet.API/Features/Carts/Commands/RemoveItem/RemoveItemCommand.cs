using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Commands.RemoveItem
{
    public class RemoveItemCommand : IRequest<BaseResponse<string>>
    {
        public RemoveItemCommand(int id)
        {
            ProductId = id;
        }
        public int ProductId { get; set; }
    }
}
