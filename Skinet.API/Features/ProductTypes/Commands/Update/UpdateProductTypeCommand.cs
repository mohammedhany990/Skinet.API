using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Commands.Update
{
    public class UpdateProductTypeCommand : IRequest<BaseResponse<string>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
