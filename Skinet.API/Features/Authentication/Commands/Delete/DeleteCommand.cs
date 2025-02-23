using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Authentication.Commands.Delete
{
    public class DeleteCommand : IRequest<BaseResponse<string>>
    {
    }
}
