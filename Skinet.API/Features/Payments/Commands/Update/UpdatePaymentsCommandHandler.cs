using AutoMapper;
using MediatR;
using Skinet.API.Features.Carts.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommandHandler : IRequestHandler<UpdatePaymentsCommand, BaseResponse<CartModel>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public UpdatePaymentsCommandHandler(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<CartModel>> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntentAsync(request.BasketId);
            if (customerBasket is null)
            {
                return new BaseResponse<CartModel>(404, false, "There's a problem with your Basket.");
            }
            var mappedBasket = _mapper.Map<CartModel>(customerBasket);
            return new BaseResponse<CartModel>(200, true, 1, mappedBasket, "Payment updated successfully");

        }
    }
}
