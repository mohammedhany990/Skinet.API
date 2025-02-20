using MediatR;
using Skinet.API.Features.Products.Models;
using Skinet.API.Helper;
using Skinet.Core.Specifications;

namespace Skinet.API.Features.Products.Queries.GetAllWithPaginationProducts
{
    public class GetAllWithPaginationProductsQuery : IRequest<Pagination<List<ProductModel>>>
    {
        public GetAllWithPaginationProductsQuery(ProductSpecificationParameters? parameters)
        {
            Parameters = parameters;
        }
        public ProductSpecificationParameters Parameters { get; set; }

    }
}
