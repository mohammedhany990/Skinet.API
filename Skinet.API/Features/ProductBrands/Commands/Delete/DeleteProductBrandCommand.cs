using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Commands.Delete
{
    public class DeleteProductBrandCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
