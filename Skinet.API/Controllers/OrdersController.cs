using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Orders.Commands.CancelOrder;
using Skinet.API.Features.Orders.Commands.Create;
using Skinet.API.Features.Orders.Commands.UpdateOrderStatus;
using Skinet.API.Features.Orders.Models;
using Skinet.API.Features.Orders.Queries.GetAllOrderStatuses;
using Skinet.API.Features.Orders.Queries.GetByIdSpecificOrderForUser;
using Skinet.API.Features.Orders.Queries.GetDeliveryMethod;
using Skinet.API.Features.Orders.Queries.GetOrdersForUser;
using Skinet.API.Helper;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class OrdersController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public OrdersController( IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<OrderModel>>> CreateOrder(CreateOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        
        //[CacheAttribute(300)]
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<OrderModel>>>> GetOrdersForUser()
        {
            var response = await _mediator.Send(new GetOrdersForUserQuery());
            return Ok(response);
        }

        //[CacheAttribute(300)]
        [MapToApiVersion("1.0")]
        [HttpGet("order-id")]
        public async Task<ActionResult<OrderModel>> GetSpecificOrderForUser(int orderId)
        {
            var response = await _mediator.Send(new GetByIdSpecificOrderForUserQuery(orderId));
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("cancel")]
        public async Task<ActionResult<OrderModel>> CancelOrderById(CancelOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("update-status")]
        public async Task<ActionResult<OrderModel>> UpdateOrderStatus(UpdateOrderStatusCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<BaseResponse<List<DeliveryMethodModel>>>> GetDeliveryMethods()
        {
            var response = await _mediator.Send(new GetDeliveryMethodsQuery());
            return Ok(response);
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<BaseResponse<List<string>>>> GetAllOrderStatuses()
        {
            var response = await _mediator.Send(new GetAllOrderStatusesQuery());
            return Ok(response);
        }
    }
}