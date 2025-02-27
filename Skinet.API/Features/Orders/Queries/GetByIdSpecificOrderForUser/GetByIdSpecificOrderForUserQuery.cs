using MediatR;
using Skinet.API.Features.Orders.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetByIdSpecificOrderForUser
{
    public class GetByIdSpecificOrderForUserQuery : IRequest<BaseResponse<OrderResponse>>
    {
        public GetByIdSpecificOrderForUserQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
