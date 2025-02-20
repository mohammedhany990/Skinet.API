using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommand, BaseResponse<string>>
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateItemQuantityCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<string>(401, false, "Unauthorized. User ID is missing.");

            var isUpdated = await _cartService.UpdateItemQuantityAsync(userId, request.ProductId, request.Quantity);



            return isUpdated
                ? new BaseResponse<string>(200, true, "Item quantity updated successfully.")
                : new BaseResponse<string>(400, false, "Failed to update item quantity.");
        }
    }
}
