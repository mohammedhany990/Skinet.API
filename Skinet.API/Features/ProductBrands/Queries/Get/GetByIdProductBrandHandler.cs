using AutoMapper;
using MediatR;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
namespace Skinet.API.Features.ProductBrands.Queries.Get
{
    public class GetByIdProductBrandHandler : IRequestHandler<GetByIdProductBrandQuery, BaseResponse<ProductBrandModel>>
    {
        private readonly IProductBrandService _productBrandService;
        private readonly IMapper _mapper;
        public GetByIdProductBrandHandler(IProductBrandService productBrandService, IMapper mapper)
        {
            _productBrandService = productBrandService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ProductBrandModel>> Handle(GetByIdProductBrandQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductBrandId <= 0)
            {
                return new BaseResponse<ProductBrandModel>
                {
                    Success = false,
                    Message = "Invalid Product Brand ID",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var brand = await _productBrandService.GetProductBrandByIdAsync(request.ProductBrandId);
            if (brand is null)
            {
                return new BaseResponse<ProductBrandModel>
                {
                    Success = false,
                    Message = "Product Brand not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var mappedBrand = _mapper.Map<ProductBrandModel>(brand);
            return new BaseResponse<ProductBrandModel>
            {
                Success = true,
                Message = "Product Brand retrieved successfully",
                Data = mappedBrand,
                StatusCode = StatusCodes.Status200OK,
                Count = 1

            };
        }
    }
}
