using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Carts.Commands.Create;
using Skinet.API.Features.Carts.Commands.Delete;
using Skinet.API.Features.Carts.Commands.RemoveItem;
using Skinet.API.Features.Carts.Commands.Update;
using Skinet.API.Features.Carts.Commands.UpdateItemQuantity;
using Skinet.API.Features.Carts.Models;
using Skinet.API.Features.Carts.Queries.GetCart;
using Skinet.API.Features.Carts.Queries.GetCartTotal;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    public class CartController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<BaseResponse<CartModel>>> GetCart()
        {
            var response = await _mediator.Send(new GetCartQuery());
            return Ok(response);
        }

        [HttpGet("get-total")]
        public async Task<ActionResult<BaseResponse<decimal>>> GetCartTotal()
        {
            var response = await _mediator.Send(new GetCartTotalQuery());
            return Ok(response);
        }


        [HttpPost("add")]
        public async Task<ActionResult<BaseResponse<string>>> AddItem([FromBody] AddToCartCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("update-quantity")]
        public async Task<ActionResult<BaseResponse<string>>> UpdateItemQuantity([FromBody] UpdateItemQuantityCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<BaseResponse<string>>> UpdateCart([FromBody] UpdateCartCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<ActionResult<BaseResponse<string>>> RemoveItem(int productId)
        {
            var response = await _mediator.Send(new RemoveItemCommand(productId));
            return Ok(response);
        }


        [HttpDelete("clear")]
        public async Task<ActionResult<BaseResponse<string>>> ClearCart()
        {
            var response = await _mediator.Send(new ClearCartCommand());
            return StatusCode(response.StatusCode, response);
        }
    }
}
