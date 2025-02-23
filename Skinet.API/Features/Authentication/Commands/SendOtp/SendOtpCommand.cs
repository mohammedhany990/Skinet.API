using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Authentication.Commands.SendOtp
{
    public class SendOtpCommand : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
    }
}
