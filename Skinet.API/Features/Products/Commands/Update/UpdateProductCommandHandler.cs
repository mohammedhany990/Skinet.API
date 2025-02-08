using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Skinet.API.DTOs;
using Skinet.API.Helper;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, BaseResponse<string>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductBrandService _productBrandService;

        public UpdateProductCommandHandler(IProductService productService,
            IMapper mapper,
            IProductTypeService productTypeService,
            IProductBrandService productBrandService)
        {
            _productService = productService;
            _mapper = mapper;
            _productTypeService = productTypeService;
            _productBrandService = productBrandService;
        }
        public async Task<BaseResponse<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

            var type = await _productTypeService.GetProductTypeByIdAsync(request.ProductTypeId ?? 0);
            if (type is null)
            {
                return new BaseResponse<string>
                {
                    Message = "Invalid product type",
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var brand = await _productBrandService.GetProductBrandByIdAsync(request.ProductBrandId ?? 0);
            if (brand is null)
            {
                return new BaseResponse<string>
                {
                    Message = "Invalid product brand",
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            existingProduct.Name = request.Name ?? existingProduct.Name;
            existingProduct.Description = request.Description ?? existingProduct.Description;
            existingProduct.Price = request.Price != default ? request.Price.Value : existingProduct.Price;
            existingProduct.ProductTypeId = request.ProductTypeId.Value;
            existingProduct.ProductType = type;
            existingProduct.ProductBrandId = request.ProductBrandId.Value;
            existingProduct.ProductBrand = brand;

            if (request?.PictureUrl is not null)
            {
                var imageName = existingProduct.PictureUrl?.Split('/').Last();

                if (!string.IsNullOrEmpty(imageName))
                {
                    FileSettings.DeleteFile(imageName, "products");
                }

                var newImagePath = FileSettings.UploadFile(request.PictureUrl, "products");

                if (string.IsNullOrEmpty(newImagePath))
                {
                    return new BaseResponse<string>
                    {
                        Message = "Failed to upload image",
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }

                existingProduct.PictureUrl = newImagePath;
            }

            var result = await _productService.UpdateProductAsync(existingProduct);

            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result
            };


        }

    }
}

