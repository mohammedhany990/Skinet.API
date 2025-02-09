using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Baskets.Commands.Delete
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, BaseResponse<string>>
    {
        private readonly IBasketService _basketService;

        public DeleteBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<BaseResponse<string>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            bool result = await _basketService.DeleteBasketAsync(request.BasketId);

            if (result)
            {
                return new BaseResponse<string>(200, true, "Basket Deleted Successfully");

            }

            return new BaseResponse<string>(404, false, "Basket not found or could not be deleted.");

        }
    }
}
