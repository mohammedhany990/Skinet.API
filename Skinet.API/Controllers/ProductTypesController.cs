using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.ProductTypes.Commands.Create;
using Skinet.API.Features.ProductTypes.Commands.Delete;
using Skinet.API.Features.ProductTypes.Commands.Update;
using Skinet.API.Features.ProductTypes.Models;
using Skinet.API.Features.ProductTypes.Queries.GetById;
using Skinet.API.Features.ProductTypes.Queries.List;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductTypesController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public ProductTypesController(IMediator mediator)
        {

            _mediator = mediator;
        }


        [MapToApiVersion("1.0")]
        [HttpGet("types")]
        public async Task<ActionResult<BaseResponse<List<ProductTypeModel>>>> Types()
        {
            var response = await _mediator.Send(new GetAllProductTypesQuery());
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ProductTypeModel>>> GetById(int id)
        {
            var response = await _mediator.Send(new GetByIdProductTypeQuery(id));
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse<string>>> Update(UpdateProductTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [MapToApiVersion("1.0")]
        [HttpDelete]
        public async Task<ActionResult<BaseResponse<string>>> Delete(DeleteProductTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Create(CreateProductTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


    }
}
