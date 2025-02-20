using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Commands.Create
{
    public class CreateProductTypeCommand : IRequest<BaseResponse<string>>
    {

        public string Name { get; set; }
    }
}
