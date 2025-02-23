using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Authentication.Commands.VerifyOtp
{
    public class VerifyOtpCommand : IRequest<BaseResponse<UserResponse>>
    {
        public string Email { get; set; }
        public string Otp { get; set; }

    }
}
