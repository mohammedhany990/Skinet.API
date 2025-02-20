using AutoMapper;
using MediatR;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductBrands.Commands.Create
{
    public class CreateProductBrandCommandHandler : IRequestHandler<CreateProductBrandCommand, BaseResponse<string>>
    {
        private readonly IProductBrandService _productBrandService;
        private readonly IMapper _mapper;

        public CreateProductBrandCommandHandler(IProductBrandService productBrandService, IMapper mapper)
        {
            _productBrandService = productBrandService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<string>> Handle(CreateProductBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _productBrandService.GetProductBrandByNameAsync(request.Name);
            if (existingBrand is not null)
            {
                return new BaseResponse<string>
                {
                    Message = "Product brand already exists",
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var mappedBrand = _mapper.Map<ProductBrand>(request);
            var result = await _productBrandService.AddProductBrandAsync(mappedBrand);
            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result
            };
        }
    }
}
