using MediatR;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, BaseResponse<string>>
    {
        private readonly IOrderService _orderService;

        public CancelOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<BaseResponse<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await _orderService.CancelOrderAsync(request.OrderId);

            int statusCode;
            bool isSuccess =false;

            switch (result)
            {
                case "Order cancelled successfully.":
                    statusCode = StatusCodes.Status200OK; 
                    isSuccess = true;
                    break;

                case "Order not found.":
                    statusCode = StatusCodes.Status404NotFound;
                    break;

                case "Order is already cancelled.":
                case "Order cannot be cancelled because it has already been shipped.":
                    statusCode = StatusCodes.Status400BadRequest; 
                    break;

                case "Failed to cancel the payment intent.":
                case "Failed to cancel the order.":
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError; 
                    break;
            }

            return new BaseResponse<string>
            {
                Success = isSuccess,
                Message = result,
                Data = isSuccess ? result : null,
                StatusCode = statusCode
            };
        }
    }
}
