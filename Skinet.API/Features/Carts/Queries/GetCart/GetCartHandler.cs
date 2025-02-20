using AutoMapper;
using MediatR;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Queries.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, BaseResponse<CartModel>>
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetCartHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<BaseResponse<CartModel>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<CartModel>(401, false, "Unauthorized. User ID is missing.");

            var cart = await _cartService.GetCartAsync(userId);

            if (cart is null)
                return new BaseResponse<CartModel>(404, false, "Cart not found.");

            var mappedCart = _mapper.Map<CartModel>(cart);

            return new BaseResponse<CartModel>(200, true, cart.Items.Count, mappedCart, "Cart retrieved successfully.");
        }

    }
}
