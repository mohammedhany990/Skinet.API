using AutoMapper;
using MediatR;
using Skinet.Core.Entities.Cart;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Commands.Update
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, BaseResponse<string>>
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCartCommandHandler(ICartService cartService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<string>(401, false, "Unauthorized. User ID is missing.");

            var cart = await _cartService.GetCartAsync(userId) ?? new Cart(userId);

            ;
            cart.ClientSecret = request.ClientSecret;
            cart.DeliveryMethodId = request.DeliveryMethodId;
            cart.IsPaymentConfirmed = request.IsPaymentConfirmed;
            cart.PaymentIntentId = request.PaymentIntentId;
            cart.ShippingPrice = request.ShippingPrice;

            cart.CalculateTotal();

            await _cartService.UpdateCartAsync(userId, cart);

            return new BaseResponse<string>(200, true, "Cart updated successfully.");
        }
    }
}
