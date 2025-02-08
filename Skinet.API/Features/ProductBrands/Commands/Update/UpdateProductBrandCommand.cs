using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Commands.Update
{
    public class UpdateProductBrandCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
