using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Baskets.Queries.Get
{
    public class GetByIdBasketQuery : IRequest<BaseResponse<BasketResponse>>
    {
        public GetByIdBasketQuery(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
