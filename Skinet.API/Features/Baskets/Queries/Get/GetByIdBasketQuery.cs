using MediatR;
using Skinet.API.Features.Baskets.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Baskets.Queries.Get
{
    public class GetByIdBasketQuery : IRequest<BaseResponse<CustomerBasketModel>>
    {
        public GetByIdBasketQuery(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
