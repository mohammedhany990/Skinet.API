using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Baskets.Commands.Create;
using Skinet.API.Features.Baskets.Commands.Delete;
using Skinet.API.Features.Baskets.Models;
using Skinet.API.Features.Baskets.Queries.Get;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{

    public class BasketController : ApiBaseController
    {
       
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }
       

        [HttpGet]
        public async Task<ActionResult<BaseResponse<CustomerBasketModel>>> GetBasketById(string id)
        {
            var response = await _mediator.Send(new GetByIdBasketQuery(id));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> UpdateOrCreateBasket(CreateBasketCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<BaseResponse<string>>> DeleteBasketAsync(string id)
        {
            var response = await _mediator.Send(new DeleteBasketCommand(id));
            return Ok(response);
        }
    }
}
