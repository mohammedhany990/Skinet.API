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
        Task<bool> IsEmailConfirmed(string email);
        Task<bool> CheckExisting(string email);

        Task<UserResponse> GetCurrentUser(string userId);
        Task<Address> GetUserAddress(string email);



        //Task<JwtSecurityToken> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
        //RefreshToken GenerateRefreshToken();



        //Task<UserDto> RefreshToken(HttpRequest request, HttpResponse response);
        //Task<BaseResponse<string>> RevokeToken(string? token, HttpRequest request);


        //Task<UserDto> RefreshTokenAsync(string refreshToken);
        //Task<bool> RevokeTokenAsync(string token);


        //Task<bool> ForgotPasswordAsync(string email);

        Task<string> ResetPassword(ResetPasswordCommand resetPasswordDto);
        Task<string> ChangePasswordAsync(ChangePasswordCommand request);

        Task<BaseResponse<UserResponse>> VerifyOtpAsync(string email, string otp);
        Task<string> SendOtp(string email);
    }
}
