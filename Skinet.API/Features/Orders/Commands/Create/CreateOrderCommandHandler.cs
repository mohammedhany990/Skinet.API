using AutoMapper;
using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Entities.Order;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Orders.Commands.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<string>>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateOrderCommandHandler(IOrderService orderService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null || !user.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return new BaseResponse<string>(401, false, "User is not authenticated.");
            }

            var buyerEmail = user?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return new BaseResponse<string>(401, false, "Email claim is missing.");
            }
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

            var mappedAddress = _mapper.Map<AddressModel, UserOrderAddress>(request.ShippingAddress);

            var result = await _orderService.CreateOrderAsync(
                userId,
                buyerEmail,
                request.DeliveryMethodId,
                mappedAddress
                );

            if (result == "Order added successfully.")
            {
                return new BaseResponse<string>(200, true, "Order created successfully.");
            }

            return new BaseResponse<string>(500, false, result);
        }
    }


}
