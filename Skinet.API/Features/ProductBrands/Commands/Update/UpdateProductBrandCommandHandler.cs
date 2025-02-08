using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Implementation;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductBrands.Commands.Update
{
    public class UpdateProductBrandCommandHandler : IRequestHandler<UpdateProductBrandCommand, BaseResponse<string>>
    {
        private readonly IProductBrandService _productBrandService;

        public UpdateProductBrandCommandHandler(IProductBrandService productBrandService)
        {
            _productBrandService = productBrandService;
        }
        public async Task<BaseResponse<string>> Handle(UpdateProductBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _productBrandService.GetProductBrandByIdAsync(request.Id);
            if (brand is null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Product Brand not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Count = 0
                };
            }
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

            brand.Name = request.Name;
            var message = await _productBrandService.UpdateProductBrandAsync(brand);

            return new BaseResponse<string>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };

        }
    }
}
