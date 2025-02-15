using AutoMapper;
using MediatR;
using Skinet.API.Errors;
using Skinet.API.Features.Baskets.Models;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommandHandler : IRequestHandler<UpdatePaymentsCommand, BaseResponse<CustomerBasketModel>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public UpdatePaymentsCommandHandler(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<CustomerBasketModel>> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntentAsync(request.BasketId);
            if (customerBasket is null)
            {
                return new BaseResponse<CustomerBasketModel>(404, false, "There's a problem with your Basket.");
            }
            var mappedBasket = _mapper.Map<CustomerBasketModel>(customerBasket);
            return new BaseResponse<CustomerBasketModel>(200, true, 1,mappedBasket, "Payment updated successfully");

        }
    }
}
