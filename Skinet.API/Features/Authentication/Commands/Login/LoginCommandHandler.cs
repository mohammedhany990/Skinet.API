using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<UserResponse>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;

        }
        public async Task<BaseResponse<UserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authService.CheckExisting(request.Email);
            if (!user)
            {
                return new BaseResponse<UserResponse>(404, false, "There is no account for this User.");
            }

            if (await _authService.IsEmailConfirmed(request.Email) == false)
            {
                return new BaseResponse<UserResponse>(400, false, "Email is not confirmed.");
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result is not null)
            {
                return new BaseResponse<UserResponse>(200, true, result, "Login Successfully.");
            }

            return new BaseResponse<UserResponse>(401, false, "Invalid email or password");
        }


    }
}
