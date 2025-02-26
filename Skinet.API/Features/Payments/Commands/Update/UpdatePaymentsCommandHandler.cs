using AutoMapper;
using MediatR;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommandHandler : IRequestHandler<UpdatePaymentsCommand, BaseResponse<CartModel>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdatePaymentsCommandHandler(IPaymentService paymentService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<CartModel>> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<CartModel>
                {
                    Success = false,
                    Message = "Unauthorized. User ID is missing.",
                    StatusCode = 401
                };
            }

            var cart = await _paymentService.CreateOrUpdatePaymentIntentAsync(userId);
            if (cart is null)
            {
                return new BaseResponse<CartModel>(404, false, "There's a problem with your Basket.");
            }
            var mappedBasket = _mapper.Map<CartModel>(cart);
            return new BaseResponse<CartModel>(200, true, 1, mappedBasket, "Payment updated successfully");

        }
    }
}
