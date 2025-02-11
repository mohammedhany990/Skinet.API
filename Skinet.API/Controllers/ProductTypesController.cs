using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Products.Commands.Update;
using Skinet.API.Features.ProductTypes.Commands.Create;
using Skinet.API.Features.ProductTypes.Commands.Delete;
using Skinet.API.Features.ProductTypes.Commands.Update;
using Skinet.API.Features.ProductTypes.Queries.GetById;
using Skinet.API.Features.ProductTypes.Queries.List;
using Skinet.API.Features.ProductTypes.Queries.Response;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductTypesController : ApiBaseController
    {
        private readonly IProductTypeService _typeService;
        private readonly IMediator _mediator;

        public ProductTypesController(IProductTypeService typeService, IMediator mediator)
        {
            _typeService = typeService;
            _mediator = mediator;
        }


        [MapToApiVersion("1.0")]
        [HttpGet("types")]
        public async Task<ActionResult<BaseResponse<List<ProductTypeResponse>>>> Types()
        {
            var response = await _mediator.Send(new GetAllProductTypesQuery());
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ProductTypeResponse>>> GetById(int id)
        {
            var response = await _mediator.Send(new GetByIdProductTypeQuery(id));
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse<string>>> Update(UpdateProductTypeCommand command)
        {
            var response = await _mediator.Send( command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        public async Task<ActionResult<BaseResponse<string>>> Delete(DeleteProductTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Create(CreateProductTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


    }
}
