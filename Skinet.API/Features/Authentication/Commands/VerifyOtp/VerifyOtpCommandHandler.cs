using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
namespace Skinet.API.Features.Authentication.Commands.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, BaseResponse<UserResponse>>
    {
        private readonly IAuthService _authService;

        public VerifyOtpCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<BaseResponse<UserResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await _authService.VerifyOtpAsync(request.Email, request.Otp);

        }
    }
}
