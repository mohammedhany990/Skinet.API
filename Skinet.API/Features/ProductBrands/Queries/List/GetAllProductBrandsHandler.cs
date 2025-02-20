using AutoMapper;
using MediatR;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
namespace Skinet.API.Features.ProductBrands.Queries.List
{
    public class GetAllProductBrandsHandler : IRequestHandler<GetAllProductBrandsQuery, BaseResponse<List<ProductBrandModel>>>
    {
        private readonly IProductBrandService _productBrandService;
        private readonly IMapper _mapper;

        public GetAllProductBrandsHandler(IProductBrandService productBrandService, IMapper mapper)
        {
            _productBrandService = productBrandService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<ProductBrandModel>>> Handle(GetAllProductBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _productBrandService.GetProductBrandsAsync();
            if (brands is null || !brands.Any())
            {
                return new BaseResponse<List<ProductBrandModel>>
                {
                    Success = false,
                    Message = "Product Brands not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = new List<ProductBrandModel>()
                };
            }

            var mappedBrands = _mapper.Map<List<ProductBrandModel>>(brands);
            return new BaseResponse<List<ProductBrandModel>>
            {
                Success = true,
                Message = "Product Brands retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = mappedBrands,
                Count = mappedBrands.Count
            };
        }
    }
}
