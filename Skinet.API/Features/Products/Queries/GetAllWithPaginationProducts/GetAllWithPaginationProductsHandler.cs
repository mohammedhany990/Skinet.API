using AutoMapper;
using Azure;
using MediatR;
using Skinet.API.DTOs;
using Skinet.API.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Queries.GetAllWithPaginationProducts
{
    public class GetAllWithPaginationProductsHandler : IRequestHandler<GetAllWithPaginationProductsQuery, Pagination<List<ProductToReturnDto>>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllWithPaginationProductsHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<Pagination<List<ProductToReturnDto>>> Handle(GetAllWithPaginationProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetProductsAsync(request.Parameters);
            if (products is null || !products.Any())
            {
                return new Pagination<List<ProductToReturnDto>>
                {
                    Success = false,
                    Message = "No products found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = new List<ProductToReturnDto>(),
                    Count = 0,
                    PageIndex = request.Parameters.PageIndex,
                    PageSize = request.Parameters.PageSize
                };
            }
            var mappedProducts = _mapper.Map<List<ProductToReturnDto>>(products);
            var count = await _productService.GetProductCountAsync(request.Parameters);
            return new Pagination<List<ProductToReturnDto>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = mappedProducts,
                StatusCode = StatusCodes.Status200OK,
                Count = count,
                PageIndex = request.Parameters.PageIndex,
                PageSize = request.Parameters.PageSize
            };
        }
    }
}
