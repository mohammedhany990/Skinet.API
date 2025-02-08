using AutoMapper;
using MediatR;
using Skinet.API.DTOs;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Queries.GetAll
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, BaseResponse<List<ProductToReturnDto>>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductToReturnDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {

            var products = await _productService.GetProductsAsync(request.Parameters);

            if (products is null || !products.Any())
            {
                return new BaseResponse<List<ProductToReturnDto>>
                {
                    Success = false,
                    Message = "No products found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = new List<ProductToReturnDto>(),
                    Count = 0
                };
            }

            var count = await _productService.GetProductCountAsync(request.Parameters);

            var mappedProducts = _mapper.Map<List<ProductToReturnDto>>(products);

            return new BaseResponse<List<ProductToReturnDto>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = mappedProducts,
                StatusCode = StatusCodes.Status200OK,
                Count = count
            };
        }
    }
}
