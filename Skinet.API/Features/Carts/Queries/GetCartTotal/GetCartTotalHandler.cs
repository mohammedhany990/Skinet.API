using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Queries.GetCartTotal
{
    public class GetCartTotalHandler : IRequestHandler<GetCartTotalQuery, BaseResponse<decimal>>
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCartTotalHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<decimal>> Handle(GetCartTotalQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<decimal>(401, false, "Unauthorized. User ID is missing.");
            }

            var total = await _cartService.GetCartTotalAsync(userId);

            return new BaseResponse<decimal>(200, true, total, "Cart total retrieved successfully.");
        }
    }
}
