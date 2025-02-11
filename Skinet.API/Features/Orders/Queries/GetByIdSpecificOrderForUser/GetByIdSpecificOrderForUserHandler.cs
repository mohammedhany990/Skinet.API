using AutoMapper;
using MediatR;
using Skinet.API.DTOs.Order;
using Skinet.API.Errors;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Orders.Queries.GetByIdSpecificOrderForUser
{
    public class GetByIdSpecificOrderForUserHandler : IRequestHandler<GetByIdSpecificOrderForUserQuery, BaseResponse<OrderModel>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetByIdSpecificOrderForUserHandler(IMapper mapper, IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<OrderModel>> Handle(GetByIdSpecificOrderForUserQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null || !user.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return new BaseResponse<OrderModel>(401, false, "User is not authenticated.");
            }

            var buyerEmail = user.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(buyerEmail))
            {
                return new BaseResponse<OrderModel>(401, false, "Email claim is missing.");
            }

            var order = await _orderService.GetOrderByIdAsync(request.Id, buyerEmail);

            if (order is null)
            {
                return new BaseResponse<OrderModel>(404, false, $"There is no Order with ID {request.Id} for this user.");
            }

            var mappedOrder = _mapper.Map<OrderModel>(order);

            return new BaseResponse<OrderModel>(200, true, 1,mappedOrder, "Order retrieved successfully."); 
        }
    }

}
