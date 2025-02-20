using MediatR;
using Skinet.API.Features.Products.Models;
using Skinet.Core.Helper;
using Skinet.Core.Specifications;

namespace Skinet.API.Features.Products.Queries.GetAll
{
    public class GetAllProductsQuery : IRequest<BaseResponse<List<ProductModel>>>
    {
        public GetAllProductsQuery(ProductSpecificationParameters? parameters)
        {
            Parameters = parameters;
        }
        public ProductSpecificationParameters Parameters { get; set; }

    }
}
