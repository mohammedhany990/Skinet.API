using AutoMapper;
using MediatR;
using Skinet.API.Features.ProductTypes.Queries.Response;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductTypes.Queries.List
{
    public class GetAllProductTypesHandler : IRequestHandler<GetAllProductTypesQuery, BaseResponse<List<ProductTypeResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IProductTypeService _productTypeService;

        public GetAllProductTypesHandler(IMapper mapper, IProductTypeService productTypeService)
        {
            _mapper = mapper;
            _productTypeService = productTypeService;
        }

        public async Task<BaseResponse<List<ProductTypeResponse>>> Handle(GetAllProductTypesQuery request, CancellationToken cancellationToken)
        {

            var types = await _productTypeService.GetProductTypesAsync();

            if (types is null || !types.Any())
            {
                return new BaseResponse<List<ProductTypeResponse>>
                {
                    Success = false,
                    Message = "Product Types not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Data = new List<ProductTypeResponse>()
                };
            }

            var mappedTypes = _mapper.Map<List<ProductTypeResponse>>(types);

            return new BaseResponse<List<ProductTypeResponse>>
            {
                Success = true,
                Message = "Product Types retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = mappedTypes,
                Count = mappedTypes.Count
            };

        }
    }

}
