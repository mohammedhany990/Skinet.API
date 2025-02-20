using AutoMapper;
using MediatR;
using Skinet.API.Helper;
using Skinet.Core.Entities;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Products.Commands.Create
{

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseResponse<string>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductBrandService _productBrandService;

        public CreateProductCommandHandler(IProductService productService,
            IMapper mapper,
            IProductTypeService productTypeService,
            IProductBrandService productBrandService)
        {
            _productService = productService;
            _mapper = mapper;
            _productTypeService = productTypeService;
            _productBrandService = productBrandService;
        }
        public async Task<BaseResponse<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var type = await _productTypeService.GetProductTypeByIdAsync(request.ProductTypeId);
            if (type is null)
            {
                return new BaseResponse<string>
                {
                    Message = "Invalid Product Type",
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            var brand = await _productBrandService.GetProductBrandByIdAsync(request.ProductBrandId);
            if (brand is null)
            {
                return new BaseResponse<string>
                {
                    Message = "Invalid Product Brand",
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            var product = _mapper.Map<Product>(request);
            product.ProductType = type;
            product.ProductBrand = brand;

            if (request.PictureUrl is not null)
            {
                var imagePath = FileSettings.UploadFile(request.PictureUrl, "products");
                if (string.IsNullOrEmpty(imagePath))
                {
                    return new BaseResponse<string>
                    {
                        Message = "Failed to upload image",
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                product.PictureUrl = imagePath;
            }

            var result = await _productService.AddProductAsync(product);
            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Message = result
            };
        }
    }
}
