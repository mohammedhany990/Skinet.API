using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductTypes.Commands.Delete
{
    public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, BaseResponse<string>>
    {
        private readonly IProductTypeService _productTypeService;

        public DeleteProductTypeCommandHandler(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }
        public  async Task<BaseResponse<string>> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
        {
            var type = await _productTypeService.GetProductTypeByIdAsync(request.Id);
            if (type is null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Product Type not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Count = 0
                };
            }
            var message = await _productTypeService.DeleteProductTypeAsync(type);


            return new BaseResponse<string>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };

        }
    }
}
