﻿using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Skinet.API.DTOs.Order;
using Skinet.API.Errors;
using Skinet.API.Features.Orders.Commands.Create;
using Skinet.API.Features.Orders.Models;
using Skinet.API.Features.Orders.Queries.GetOrdersForUser;
using Skinet.Core.Entities.Order;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{

    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public OrdersController(IOrderService orderService, IMapper mapper, IMediator mediator)
        {
            _orderService = orderService;
            _mapper = mapper;
            _mediator = mediator;
        }

       
        [HttpPost]
        public async Task<ActionResult<BaseResponse<OrderModel>>> CreateOrder(CreateOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<OrderModel>>>> GetOrdersForUser()
        {
            var response = await _mediator.Send(new GetOrdersForUserQuery());
            return Ok(response);
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

