using AutoMapper;
using MediatR;
using Skinet.API.Features.ProductBrands.Responses;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
namespace Skinet.API.Features.ProductBrands.Queries.Get
{
    public class GetByIdProductBrandHandler : IRequestHandler<GetByIdProductBrandQuery, BaseResponse<ProductBrandResponse>>
    {
        private readonly IProductBrandService _productBrandService;
        private readonly IMapper _mapper;
        public GetByIdProductBrandHandler(IProductBrandService productBrandService, IMapper mapper)
        {
            _productBrandService = productBrandService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ProductBrandResponse>> Handle(GetByIdProductBrandQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductBrandId <= 0)
            {
                return new BaseResponse<ProductBrandResponse>
                {
                    Success = false,
                    Message = "Invalid Product Brand ID",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var brand = await _productBrandService.GetProductBrandByIdAsync(request.ProductBrandId);
            if (brand is null)
            {
                return new BaseResponse<ProductBrandResponse>
                {
                    Success = false,
                    Message = "Product Brand not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var mappedBrand = _mapper.Map<ProductBrandResponse>(brand);
            return new BaseResponse<ProductBrandResponse>
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
