using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Products.Commands.Delete
{
    public class DeleteProductCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
