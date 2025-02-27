using MediatR;
using Skinet.API.Features.Products.Responses;
using Skinet.Core.Helper;
using Skinet.Core.Specifications;

namespace Skinet.API.Features.Products.Queries.GetAll
{
    public class GetAllProductsQuery : IRequest<BaseResponse<List<ProductResponse>>>
    {
        public GetAllProductsQuery(ProductSpecificationParameters? parameters)
        {
            Parameters = parameters;
        }
        public ProductSpecificationParameters Parameters { get; set; }

    }
}
