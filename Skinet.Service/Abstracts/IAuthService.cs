using Skinet.API.DTOs.Identity;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Helper;
namespace Skinet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<string>> RegisterAsync(RegisterCommand request);

        Task<UserResponse> LoginAsync(string email, string password);

        Task<bool> DeleteAccountAsync(string email);

        Task<bool> IsEmailConfirmedAsync(string email);

        Task<bool> IsUserExistsByIdAsync(string userId);

        Task<bool> CheckExistingUserByEmailAsync(string email);


        Task<UserResponse> GetCurrentUserAsync(string userId);



        //Task<UserDto> RefreshToken(HttpRequest request, HttpResponse response);
        //Task<BaseResponse<string>> RevokeToken(string? token, HttpRequest request);


        //Task<UserDto> RefreshTokenAsync(string refreshToken);
        //Task<bool> RevokeTokenAsync(string token);


        Task<string> ResetPasswordAsync(ResetPasswordCommand resetPasswordDto);
        Task<string> ChangePasswordAsync(ChangePasswordCommand request);

        Task<BaseResponse<UserResponse>> VerifyOtpAsync(string email, string otp);
        Task<string> SendOtpAsync(string email);
    }
}
