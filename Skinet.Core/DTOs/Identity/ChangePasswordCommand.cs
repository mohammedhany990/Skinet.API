using MediatR;
using Skinet.Core.Helper;

namespace Skinet.Core.DTOs.Identity
{
    public class ChangePasswordCommand : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
