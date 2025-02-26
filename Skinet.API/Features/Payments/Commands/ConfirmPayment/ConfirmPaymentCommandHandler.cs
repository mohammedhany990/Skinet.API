using AutoMapper;
using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Payments.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, BaseResponse<OrderModel>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public ConfirmPaymentCommandHandler(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<OrderModel>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _paymentService.ConfirmPaymentAndUpdateOrderAsync(request.PaymentIntentId);

            if (order == null)
            {
                return new BaseResponse<OrderModel>(404, false, 0, null, "Order not found.");
            }

            var mappedOrder = _mapper.Map<OrderModel>(order);

            return new BaseResponse<OrderModel>(200, true, 1, mappedOrder, "Payment confirmed successfully.");
        }
    }
}
