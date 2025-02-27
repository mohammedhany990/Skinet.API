using AutoMapper;
using MediatR;
using Skinet.API.Features.ProductTypes.Responses;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductTypes.Queries.GetById
{
    public class GetByIdProductTypeHandler : IRequestHandler<GetByIdProductTypeQuery, BaseResponse<ProductTypeResponse>>
    {
        private readonly IProductTypeService _typeService;
        private readonly IMapper _mapper;

        public GetByIdProductTypeHandler(IProductTypeService typeService, IMapper mapper)
        {
            _typeService = typeService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ProductTypeResponse>> Handle(GetByIdProductTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.ProductTypeId <= 0)
            {
                return new BaseResponse<ProductTypeResponse>
                {
                    Success = false,
                    Message = "Invalid Product Type ID",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var type = await _typeService.GetProductTypeByIdAsync(request.ProductTypeId);

            if (type is null)
            {
                return new BaseResponse<ProductTypeResponse>
                {
                    Success = false,
                    Message = "Product Type not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var mappedType = _mapper.Map<ProductTypeResponse>(type);

            return new BaseResponse<ProductTypeResponse>
            {
                Success = true,
                Message = "Product Type retrieved successfully",
                Data = mappedType,
                StatusCode = StatusCodes.Status200OK,
                Count = 1
            };
        }
    }


}
