using AutoMapper;
using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using System.Security.Claims;
using Skinet.Core.Entities.Order;
using Skinet.API.DTOs.Order;

namespace Skinet.API.Features.Orders.Commands.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<OrderModel>>
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
        public async Task<BaseResponse<OrderModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null || !user.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return new BaseResponse<OrderModel>(401, false, "User is not authenticated.");
            }

            var buyerEmail = user?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return new BaseResponse<OrderModel>(401, false, "Email claim is missing.");
            }

            var mappedAddress = _mapper.Map<AddressModel, UserOrderAddress>(request.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail,
                request.DeliveryMethodId,
                request.BasketId,
                mappedAddress);

            if (order is null)
            {
                return new BaseResponse<OrderModel>(500, false, "Order creation failed.");
            }

            var mappedOrder = _mapper.Map<Order, OrderModel>(order);

            return new BaseResponse<OrderModel>(200, true, 1, mappedOrder, "Order created successfully.");
        }
    }
}
