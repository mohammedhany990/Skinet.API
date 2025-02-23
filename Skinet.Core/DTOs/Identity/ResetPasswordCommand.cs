using MediatR;
using Skinet.Core.Helper;

namespace Skinet.Core.DTOs.Identity
{
    public class ResetPasswordCommand : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Otp { get; set; }
    }
}
