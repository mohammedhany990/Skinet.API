using MediatR;
using Skinet.API.Features.Orders.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Orders.Queries.GetByIdSpecificOrderForUser
{
    public class GetByIdSpecificOrderForUserQuery : IRequest<BaseResponse<OrderModel>>
    {
        public GetByIdSpecificOrderForUserQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
