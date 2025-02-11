using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.ProductBrands.Commands.Create;
using Skinet.API.Features.ProductBrands.Commands.Delete;
using Skinet.API.Features.ProductBrands.Commands.Update;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.API.Features.ProductBrands.Queries.Get;
using Skinet.API.Features.ProductBrands.Queries.List;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductBrandsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public ProductBrandsController( IMediator mediator)
        {
           
            _mediator = mediator;
        }
        [MapToApiVersion("1.0")]
        [HttpGet("brands")]
        public async Task<ActionResult<BaseResponse<List<ProductBrandModel>>>> Brands()
        {
            var response = await _mediator.Send(new GetAllProductBrandsQuery());
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ProductBrandModel>>> Brand(int id)
        {
            var response = await _mediator.Send(new GetByIdProductBrandQuery(id));
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Create(CreateProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Delete(DeleteProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<string>>> Update(UpdateProductBrandCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
