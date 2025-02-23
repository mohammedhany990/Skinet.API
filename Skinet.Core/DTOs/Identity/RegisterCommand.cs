using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.DTOs.Identity
{
    public class RegisterCommand : IRequest<BaseResponse<string>>
    {
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
