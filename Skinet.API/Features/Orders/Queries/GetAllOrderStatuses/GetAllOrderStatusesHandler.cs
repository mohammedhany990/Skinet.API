using MediatR;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Orders.Queries.GetAllOrderStatuses
{
    public class GetAllOrderStatusesHandler : IRequestHandler<GetAllOrderStatusesQuery, BaseResponse<List<string>>>
    {
        private readonly IOrderService _odeOrderService;

        public GetAllOrderStatusesHandler(IOrderService odeOrderService)
        {
            _odeOrderService = odeOrderService;
        }
        public async  Task<BaseResponse<List<string>>> Handle(GetAllOrderStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _odeOrderService.GetAllOrderStatuses();
            if (statuses is null || !statuses.Any())
            {
                return new BaseResponse<List<string>>
                {
                    Success = false,
                    Message = "Order statuses not found.",
                    StatusCode = 404,
                };
            }
            return new BaseResponse<List<string>>
            {
                Success = true,
                Message = "Order statuses retrieved successfully.",
                Data = statuses,
                StatusCode = 200,
                Count = statuses.Count
            };
            
        }
    }
}
