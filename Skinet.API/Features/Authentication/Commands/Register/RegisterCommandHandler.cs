using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
namespace Skinet.API.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<BaseResponse<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingEmail = await _authService.CheckExistingUserByEmailAsync(request.Email);

            if (existingEmail && await _authService.IsEmailConfirmedAsync(request.Email) == false)
            {
                return new BaseResponse<string>(400, false, "Email is already registered, but you need to confirm.");
            }
            if (existingEmail)
            {
                return new BaseResponse<string>(400, false, "Email is already registered.");
            }

            return await _authService.RegisterAsync(request);
        }
    }
}
