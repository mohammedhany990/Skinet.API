using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Baskets.Commands.Delete
{
    public class DeleteBasketCommand : IRequest<BaseResponse<string>>
    {
        public DeleteBasketCommand(string id)
        {
            BasketId = id;
        }
        public string BasketId { get; set; }
    }
}
