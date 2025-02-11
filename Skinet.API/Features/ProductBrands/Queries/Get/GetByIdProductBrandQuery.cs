using MediatR;
using Skinet.API.Features.ProductBrands.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Queries.Get
{
    public class GetByIdProductBrandQuery:IRequest<BaseResponse<ProductBrandModel>>
    {
        public GetByIdProductBrandQuery(int id)
        {
            ProductBrandId = id;
        }
        public int ProductBrandId { get; set; }
    }
}
