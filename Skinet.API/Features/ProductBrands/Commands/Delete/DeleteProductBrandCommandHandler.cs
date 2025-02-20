using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductBrands.Commands.Delete
{
    public class DeleteProductBrandCommandHandler : IRequestHandler<DeleteProductBrandCommand, BaseResponse<string>>
    {
        private readonly IProductBrandService _productBrandService;

        public DeleteProductBrandCommandHandler(IProductBrandService productBrandService)
        {
            _productBrandService = productBrandService;
        }
        public async Task<BaseResponse<string>> Handle(DeleteProductBrandCommand request, CancellationToken cancellationToken)
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
            var message = await _productBrandService.DeleteProductBrandAsync(brand);
            return new BaseResponse<string>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };

        }
    }
}
