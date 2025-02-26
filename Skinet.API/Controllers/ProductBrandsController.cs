using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.ProductBrands.Commands.Create;
using Skinet.API.Features.ProductBrands.Commands.Delete;
using Skinet.API.Features.ProductBrands.Commands.Update;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.API.Features.ProductBrands.Queries.Get;
using Skinet.API.Features.ProductBrands.Queries.List;
using Skinet.API.Helper;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductBrandsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public ProductBrandsController(IMediator mediator)
        {

            _mediator = mediator;
        }
        [CacheAttribute(300)]
        [MapToApiVersion("1.0")]
        [HttpGet("brands")]
        public async Task<ActionResult<BaseResponse<List<ProductBrandModel>>>> Brands()
        {
            var response = await _mediator.Send(new GetAllProductBrandsQuery());
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ProductBrandModel>>> Brand([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetByIdProductBrandQuery(id));
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Create(CreateProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Delete(DeleteProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Update(UpdateProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
