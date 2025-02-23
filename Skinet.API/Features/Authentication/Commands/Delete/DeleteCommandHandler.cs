using MediatR;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Authentication.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCommandHandler(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email) || await _authService.CheckExisting(email) == false)
            {
                return new BaseResponse<string>(404, false, "User not found.");
            }

            var isDeleted = await _authService.DeleteAccountAsync(email);

            return isDeleted
                ? new BaseResponse<string>(404, false, "Account deleted successfully.")
                : new BaseResponse<string>(400, false, "Something went wrong.");
        }
    }
}
