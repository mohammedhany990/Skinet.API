using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
    }
}
