using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Commands.Delete
{
    public class DeleteProductTypeCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
