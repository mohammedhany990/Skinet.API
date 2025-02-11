using MediatR;
using Skinet.API.DTOs;
using Skinet.API.Features.Orders.Models;
using Skinet.API.Features.Products.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Products.Queries.GetById
{
    public class GetByIdProductQuery : IRequest<BaseResponse<ProductModel>>
    {
        public GetByIdProductQuery(int productId)
        {
            ProductId = productId;
        }
        public int ProductId { get; set; }
    }
}
