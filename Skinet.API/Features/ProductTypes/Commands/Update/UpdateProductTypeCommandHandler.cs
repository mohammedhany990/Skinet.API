using MediatR;

using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.ProductTypes.Commands.Update
{

    public class UpdateProductTypeCommandHandler : IRequestHandler<UpdateProductTypeCommand, BaseResponse<string>>
    {
        private readonly IProductTypeService _productTypeService;

        public UpdateProductTypeCommandHandler(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        public async Task<BaseResponse<string>> Handle(UpdateProductTypeCommand request,
            CancellationToken cancellationToken)
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

            type.Name = request.Name;

            var message = await _productTypeService.UpdateProductTypeAsync(type);

            return new BaseResponse<string>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };

        }
    }

}
