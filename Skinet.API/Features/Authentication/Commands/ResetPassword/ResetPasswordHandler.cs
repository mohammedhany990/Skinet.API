using MediatR;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Authentication.Commands.ResetPassword
{

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;

        public ResetPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<BaseResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPassword(request);

            if (result == "Invalid or expired OTP." || result == "Failed to reset password.")
            {
                return new BaseResponse<string>(400, false, result);
            }

            return new BaseResponse<string>(200, true, result);

        }
    }
}
