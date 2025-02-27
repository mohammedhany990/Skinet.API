using AutoMapper;
using MediatR;
using Skinet.API.Features.Carts.Responses;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommandHandler : IRequestHandler<UpdatePaymentsCommand, BaseResponse<CartResponse>>
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
        public async Task<BaseResponse<CartResponse>> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<CartResponse>
                {
                    Success = false,
                    Message = "Unauthorized. User ID is missing.",
                    StatusCode = 401
                };
            }

            var cart = await _paymentService.CreateOrUpdatePaymentIntentAsync(userId);
            if (cart is null)
            {
                return new BaseResponse<CartResponse>(404, false, "There's a problem with your Basket.");
            }
            var mappedBasket = _mapper.Map<CartResponse>(cart);
            return new BaseResponse<CartResponse>(200, true, 1, mappedBasket, "Payment updated successfully");

        }
    }
}
