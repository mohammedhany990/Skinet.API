using AutoMapper;
using MediatR;
using Skinet.API.Features.Products.Responses;
using Skinet.API.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Queries.GetAllWithPaginationProducts
{
    public class GetAllWithPaginationProductsHandler : IRequestHandler<GetAllWithPaginationProductsQuery, Pagination<List<ProductResponse>>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllWithPaginationProductsHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<Pagination<List<ProductResponse>>> Handle(GetAllWithPaginationProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetProductsAsync(request.Parameters);
            if (products is null || !products.Any())
            {
                return new Pagination<List<ProductResponse>>
                {
                    Success = false,
                    Message = "No products found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = new List<ProductResponse>(),
                    Count = 0,
                    PageIndex = request.Parameters.PageIndex,
                    PageSize = request.Parameters.PageSize
                };
            }
            var mappedProducts = _mapper.Map<List<ProductResponse>>(products);
            var count = await _productService.GetProductCountAsync(request.Parameters);
            return new Pagination<List<ProductResponse>>
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
