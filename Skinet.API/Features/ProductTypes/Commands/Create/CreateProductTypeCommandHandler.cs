using AutoMapper;
using MediatR;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductTypes.Commands.Create
{
    public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, BaseResponse<string>>
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IMapper _mapper;

        public CreateProductTypeCommandHandler(IProductTypeService productTypeService, IMapper mapper)
        {
            _productTypeService = productTypeService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<string>> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var existingType = await _productTypeService.GetProductTypeByNameAsync(request.Name);
            if (existingType is not null)
            {
                return new BaseResponse<string>
                {
                    Message = "Product type already exists",
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var productType = _mapper.Map<ProductType>(request);
            var result = await _productTypeService.AddProductTypeAsync(productType);

            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result
            };
        }
    }
}
