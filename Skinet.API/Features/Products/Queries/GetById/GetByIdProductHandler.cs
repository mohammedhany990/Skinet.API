using AutoMapper;
using MediatR;
using Skinet.API.DTOs;
using Skinet.API.Features.ProductTypes.Queries.Response;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Queries.GetById
{
    public class GetByIdProductHandler : IRequestHandler<GetByIdProductQuery, BaseResponse<ProductToReturnDto>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetByIdProductHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ProductToReturnDto>> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductId <= 0)
            {
                return new BaseResponse<ProductToReturnDto>
                {
                    Success = false,
                    Message = "Invalid Product ID",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product is null)
            {
                return new BaseResponse<ProductToReturnDto>
                {
                    Message = "Product not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false
                };
            }
            return new BaseResponse<ProductToReturnDto>
            {
                Data = _mapper.Map<ProductToReturnDto>(product),
                Message = "Product found successfully",
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Count = 1
            };
        }
    }
}
