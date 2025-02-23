using MediatR;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;

        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<BaseResponse<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var existing = await _authService.CheckExisting(request.Email);
            if (!existing)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "This email is not registered."
                };
            }

            var result = await _authService.SendOtp(request.Email);
            if (result == "OTP has been sent to your email")
            {
                return new BaseResponse<string>(200, true, result);
            }
            return new BaseResponse<string>(400, false, result);

        }
    }
}
