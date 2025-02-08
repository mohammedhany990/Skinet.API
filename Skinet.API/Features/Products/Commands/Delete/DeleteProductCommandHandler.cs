using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, BaseResponse<string>>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<BaseResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.Id < 0)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Invalid Product ID",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var existingProduct = await _productService.GetProductByIdAsync(request.Id);
            if (existingProduct is null)
            {
                return new BaseResponse<string>
                {
                    Message = "Product not found",
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var result = await _productService.DeleteProductAsync(existingProduct);

            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result
            };
        }
    }
}
