using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Products.Commands.Create;
using Skinet.API.Features.Products.Commands.Delete;
using Skinet.API.Features.Products.Commands.Update;
using Skinet.API.Features.Products.Queries.GetAll;
using Skinet.API.Features.Products.Queries.GetAllWithPaginationProducts;
using Skinet.API.Features.Products.Queries.GetById;
using Skinet.API.Features.Products.Responses;
using Skinet.API.Helper;
using Skinet.Core.Helper;
using Skinet.Core.Specifications;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<ProductResponse>>>> GetAllProducts([FromQuery] ProductSpecificationParameters? parameters)
        {
            var response = await _mediator.Send(new GetAllProductsQuery(parameters));
            return Ok(response);
        }

        [CacheAttribute(300)]
        [MapToApiVersion("1.0")]
        [HttpGet("get-all")]
        public async Task<ActionResult<Pagination<List<ProductResponse>>>> GetAllProductsWithPagination([FromQuery] ProductSpecificationParameters? parameters)
        {
            var response = await _mediator.Send(new GetAllWithPaginationProductsQuery(parameters));
            return Ok(response);
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<ProductResponse>>> GetProduct(int id)
        {
            var response = await _mediator.Send(new GetByIdProductQuery(id));
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<string>>> CreateProduct([FromForm] CreateProductCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse<string>>> UpdateProduct([FromForm] UpdateProductCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse<string>>> UpdateProduct([FromForm] DeleteProductCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
