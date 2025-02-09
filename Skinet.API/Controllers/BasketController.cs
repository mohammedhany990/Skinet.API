using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Folder;
using Skinet.API.Errors;
using Skinet.API.Features.Baskets.Commands.Create;
using Skinet.API.Features.Baskets.Commands.Delete;
using Skinet.API.Features.Baskets.Queries;
using Skinet.API.Features.Baskets.Queries.Get;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<ActionResult<BaseResponse<BasketResponse>>> GetBasketById(string id)
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
