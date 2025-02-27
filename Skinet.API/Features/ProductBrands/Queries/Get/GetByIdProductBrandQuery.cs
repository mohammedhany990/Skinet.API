using MediatR;
using Skinet.API.Features.ProductBrands.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Queries.Get
{
    public class GetByIdProductBrandQuery : IRequest<BaseResponse<ProductBrandResponse>>
    {
        public GetByIdProductBrandQuery(int id)
        {
            ProductBrandId = id;
        }
        public int ProductBrandId { get; set; }
    }
}
