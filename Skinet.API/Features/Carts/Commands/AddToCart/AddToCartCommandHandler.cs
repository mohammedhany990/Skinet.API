using AutoMapper;
using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Carts.Commands.Create
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, BaseResponse<string>>
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddToCartCommandHandler(ICartService cartService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Unauthorized. User ID is missing.",
                    StatusCode = 401
                };
            }

            var result = await _cartService.AddItemToCartAsync(userId, request.ProductId, request.Quantity);

            if (!result)
            {
                return new BaseResponse<string>(400, false, "Failed to add item to cart.");
            }

            return new BaseResponse<string>(200, true, "Item added successfully.");

        }
    }
}
