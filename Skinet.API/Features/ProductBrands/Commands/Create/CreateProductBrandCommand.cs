using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Commands.Create
{
    public class CreateProductBrandCommand : IRequest<BaseResponse<string>>
    {
        public string Name { get; set; }
    }
}
