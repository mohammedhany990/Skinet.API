using MediatR;
using Skinet.API.Features.ProductTypes.Queries.Response;
using Skinet.Core.Entities;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Queries.GetById
{
    public class GetByIdProductTypeQuery : IRequest<BaseResponse<ProductTypeResponse>>
    {
        public GetByIdProductTypeQuery(int id)
        {
            ProductTypeId = id;
        }
        public int ProductTypeId { get; set; }
    }
}
