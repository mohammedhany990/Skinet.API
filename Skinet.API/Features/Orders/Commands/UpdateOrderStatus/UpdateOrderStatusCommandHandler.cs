using MediatR;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Orders.Commands.UpdateOrderStatus
{

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, BaseResponse<string>>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderStatusCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<BaseResponse<string>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _orderService.UpdateOrderStatusAsync(request.OrderId, request.Status);

            int statusCode;
            bool isSuccess = false;

            switch (result)
            {
                case string s when s.StartsWith("Success:"):
                    statusCode = StatusCodes.Status200OK; // 200
                    isSuccess = true;
                    break;

                case "Order Not Found.":
                    statusCode = StatusCodes.Status404NotFound; // 404
                    break;

                case "Invalid Status Transition.":
                case "Invalid Status: The provided status is not recognized.":
                    statusCode = StatusCodes.Status400BadRequest; // 400
                    break;

                case "Save Failed: Unable to save changes to the database.":
                case "Error: An error occurred while updating the order status.":
                    statusCode = StatusCodes.Status500InternalServerError; // 500
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError; // 500
                    break;
            }

            // Return a BaseResponse<string> with the appropriate status code and message
            return new BaseResponse<string>
            {
                Success = isSuccess,
                Message = result,
                Data = isSuccess ? result : null, // Optionally include data if needed
                StatusCode = statusCode
            };
        }
    }
}
