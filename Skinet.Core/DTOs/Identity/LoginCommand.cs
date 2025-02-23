using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.DTOs.Identity
{
    public class LoginCommand : IRequest<BaseResponse<UserResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
