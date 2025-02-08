using MediatR;
using Skinet.API.DTOs;
using Skinet.API.Helper;
using Skinet.Core.Helper;
using Skinet.Core.Specifications;

namespace Skinet.API.Features.Products.Queries.GetAll
{
    public class GetAllProductsQuery  :IRequest<BaseResponse<List<ProductToReturnDto>>>
    {
        public GetAllProductsQuery(ProductSpecificationParameters? parameters)
        {
            Parameters = parameters;
        }
        public ProductSpecificationParameters Parameters { get; set; }

    }
}
