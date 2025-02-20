using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Commands.RemoveItem
{
    public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, BaseResponse<string>>
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveItemCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<string>(401, false, "Unauthorized. User ID is missing.");


            var isRemoved = await _cartService.RemoveItemFromCartAsync(userId, request.ProductId);

            return isRemoved
                ? new BaseResponse<string>(200, true, "Item Removed successfully.")
                : new BaseResponse<string>(404, false, "Item not found or could not be deleted.");
        }
    }
}
