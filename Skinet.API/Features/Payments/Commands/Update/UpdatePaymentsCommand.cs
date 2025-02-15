using MediatR;
using Skinet.API.Features.Baskets.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommand : IRequest<BaseResponse<CustomerBasketModel>>
    {
        public string BasketId { get; set; }
       
    }
}
