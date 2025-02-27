using AutoMapper;
using MediatR;
using Skinet.API.Features.Orders.Responses;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Orders.Queries.GetDeliveryMethod
{
    public class GetDeliveryMethodsHandler : IRequestHandler<GetDeliveryMethodsQuery, BaseResponse<List<DeliveryMethodResponse>>>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public GetDeliveryMethodsHandler(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<DeliveryMethodResponse>>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetDeliveryMethodAsync();

            if (result is null || !result.Any())
            {
                return new BaseResponse<List<DeliveryMethodResponse>>(404, false, "Delivery methods not found.");
            }

            var mappedResult = _mapper.Map<List<DeliveryMethodResponse>>(result);

            return new BaseResponse<List<DeliveryMethodResponse>>(200, true, result.Count, mappedResult,
                "Delivery methods found successfully.");
        }
    }
}
