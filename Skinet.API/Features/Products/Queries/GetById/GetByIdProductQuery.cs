using MediatR;
using Skinet.API.DTOs;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Products.Queries.GetById
{
    public class GetByIdProductQuery : IRequest<BaseResponse<ProductToReturnDto>>
    {
        public GetByIdProductQuery(int productId)
        {
            ProductId = productId;
        }
        public int ProductId { get; set; }
    }
}
