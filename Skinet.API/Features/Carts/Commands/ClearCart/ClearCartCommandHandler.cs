using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Commands.Delete
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, BaseResponse<string>>
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClearCartCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<string>(401, false, "Unauthorized. User ID is missing.");


            var isCleared = await _cartService.ClearCartAsync(userId);

            return isCleared
                ? new BaseResponse<string>(200, true, "Cart deleted successfully.")
                : new BaseResponse<string>(404, false, "Cart not found or could not be deleted.");
        }

    }
}
