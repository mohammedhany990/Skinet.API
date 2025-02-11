using AutoMapper;
using MediatR;
using Skinet.API.DTOs.Order;
using Skinet.API.Errors;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Orders.Queries.GetOrdersForUser
{
    public class GetOrdersForUserHandler : IRequestHandler<GetOrdersForUserQuery, BaseResponse<List<OrderModel>>>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetOrdersForUserHandler(IOrderService orderService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<List<OrderModel>>> Handle(GetOrdersForUserQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null || !user.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return new BaseResponse<List<OrderModel>>(401, false, "User is not authenticated.");
            }

            var buyerEmail = user?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return new BaseResponse<List<OrderModel>>(401, false, "Email claim is missing.");
            }

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            if (orders is null || !orders.Any())
            {
                return new BaseResponse<List<OrderModel>>(404, false, "There are no orders.");
            }

            var mappedOrders = _mapper.Map<List<OrderModel>>(orders);

            return new BaseResponse<List<OrderModel>>(200, true, mappedOrders.Count, mappedOrders, "Orders found successfully");
        }
    }

}
