using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Skinet.API.DTOs.Order;
using Skinet.API.Errors;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{

    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var mappedAddress = _mapper.Map<AddressDto, UserOrderAddress>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail,
                orderDto.DeliveryMethodId,
                orderDto.BasketId,
                mappedAddress);


            if (order is null)
            {
                return BadRequest(new ApiResponse(400, "There's a problem with your order."));
            }
            var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mappedOrder);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            if (orders is null)
            {
                return BadRequest(new ApiResponse(400, "There are no Orders."));
            }
            if (orders?.Count() <= 0)
            {
                return NotFound(new ApiResponse(404, "There are no Orders."));
            }

            var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(MappedOrder);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetSpecificOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail);
            if (order is null)
            {
                return BadRequest(new ApiResponse(404, $"There is no Order with {id} for this user."));
            }
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(MappedOrder);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            var deliveries = await _orderService.GetDeliveryMethodsAsync();
            if (deliveries is null)
            {
                return BadRequest(new ApiResponse(404, $"There is no Delivery Methods"));
            }
            return Ok(deliveries);
        }

    }
}

