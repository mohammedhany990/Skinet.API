using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Carts.Commands.Delete
{
    public class ClearCartCommand : IRequest<BaseResponse<string>>
    {

    }
}
