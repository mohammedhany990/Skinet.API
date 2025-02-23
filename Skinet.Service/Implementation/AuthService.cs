using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skinet.API.DTOs.Identity;
using Skinet.API.ExtensionMethods;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Skinet.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSettings _emailSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDatabase _redis;

        public AuthService(IConfiguration configuration,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConnectionMultiplexer redis,
            IEmailSettings emailSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
            _httpContextAccessor = httpContextAccessor;
            _redis = redis.GetDatabase();

        }



        private void SetRefreshTokenInCookie(string refreshToken, DateTime expire)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expire.ToLocalTime(),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            httpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        /*
        public async Task<BaseResponse<UserResponse>> RegisterAsync(RegisterCommand registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = false,
                RefreshTokens = new List<RefreshToken>() 
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Errors",
                    Errors = errors
                };
            }

            var jwtSecurityToken = await CreateTokenAsync(user, _userManager);

            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens?.Add(refreshToken);

            await _userManager.UpdateAsync(user);

            await SendOtp(user.Email);

            var returnedUser = new UserResponse
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken.Token,
                ExpiresOn = jwtSecurityToken.ValidTo,
                RefreshTokenExpiration = refreshToken.ExpiresOn

            };
            SetRefreshTokenInCookie(refreshToken.Token, returnedUser.RefreshTokenExpiration);

            return new BaseResponse<UserResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "User Created Successfully, Please verify the OTP sent to your email.",
                //Data = returnedUser
            };
        }*/

        public async Task<BaseResponse<string>> RegisterAsync(RegisterCommand registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = false, // Not verified yet
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new BaseResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Errors",
                    Errors = errors
                };
            }

            var isSent = await SendOtp(user.Email); // Send OTP for verification

            if (isSent != "OTP has been sent to your email")
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = isSent
                };
            }
            return new BaseResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "User Created Successfully, Please verify the OTP sent to your email."
            };
        }

        public async Task<UserResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return null;

            var jwtSecurityToken = await CreateTokenAsync(user);

            var returnedUser = new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn = jwtSecurityToken.ValidTo
            };

            var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeToken != null)
            {
                returnedUser.RefreshToken = activeToken.Token;
                returnedUser.RefreshTokenExpiration = activeToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                returnedUser.RefreshToken = refreshToken.Token;
                returnedUser.RefreshTokenExpiration = refreshToken.ExpiresOn;

                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            if (!string.IsNullOrEmpty(returnedUser.RefreshToken))
                SetRefreshTokenInCookie(returnedUser.RefreshToken, returnedUser.RefreshTokenExpiration ?? DateTime.UtcNow.AddDays(10));


            return returnedUser;
        }

        public async Task<bool> DeleteAccountAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }


        public async Task<UserResponse> GetCurrentUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var jwtSecurityToken = await CreateTokenAsync(user);

            return new UserResponse()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
        }

        public async Task<Address> GetUserAddress(string email)
        {
            var user = await _userManager.FindUserWithAddressAsync(email);
            return user.Address;
        }

        public async Task<bool> CheckExisting(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<bool> IsEmailConfirmed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user.EmailConfirmed;
        }

        public async Task<JwtSecurityToken> CreateTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var roles = await _userManager.GetRolesAsync(user);

            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );

            return Token;

        }


        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return new RefreshToken()
            {
                Token = WebEncoders.Base64UrlEncode(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreateOn = DateTime.UtcNow
            };
        }


        #region Password Manager

        public async Task<string> ResetPassword(ResetPasswordCommand request)
        {
            var storedOtp = await _redis.StringGetAsync(request.Email);
            if (string.IsNullOrEmpty(storedOtp) || storedOtp != request.Otp)
            {
                return "Invalid or expired OTP.";
            }
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _userManager.ResetPasswordAsync(user,
                await _userManager.GeneratePasswordResetTokenAsync(user),
                request.NewPassword);

            if (!result.Succeeded)
            {

                return "Failed to reset password.";
            }

            await _redis.KeyDeleteAsync(request.Email);
            return "Password has been reset successfully.";

        }


        public async Task<string> ChangePasswordAsync(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
            if (!passwordCheck)
            {
                return "Current password is incorrect.";
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return "Failed to change password.";
            }

            return "Password has been changed successfully.";
        }

        #endregion

        #region Otp Manager
        private string GenerateOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);

            // Convert bytes to an integer and ensure a 6-digit OTP
            int otp = (int)(BitConverter.ToUInt32(bytes, 0) % 900000) + 100000;

            return otp.ToString();
        }


        private async Task StoreOtpAsync(string email, string otp)
        {
            // Remove existing OTP if it exists
            await _redis.KeyDeleteAsync(email);

            // Store the new OTP with a 5-minute expiration
            await _redis.StringSetAsync(email, otp, TimeSpan.FromMinutes(5));
        }


        public async Task<string> SendOtp(string email)
        {
            try
            {
                var otp = GenerateOtp();

                await StoreOtpAsync(email, otp);
                var emailToSend = new Email()
                {
                    To = email,
                    Subject = "OTP Code",
                    Body = $"Your OTP code is {otp}",
                };

                _emailSettings.SendEmail(emailToSend);

                return "OTP has been sent to your email";
            }
            catch (Exception ex)
            {
                return $"Otb can't be sent. Error:{ex.Message}";
            }

        }


        public async Task<BaseResponse<UserResponse>> VerifyOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found."
                };
            }

            if (user.EmailConfirmed)
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Email is already verified."
                };
            }

            var storedOtp = await _redis.StringGetAsync(email);
            if (string.IsNullOrEmpty(storedOtp))
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "OTP has expired or does not exist."
                };
            }

            if (storedOtp != otp)
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid OTP."
                };
            }

            // Mark email as verified
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            // Delete OTP after successful verification
            await _redis.KeyDeleteAsync(email);

            // Generate JWT Token
            var jwtSecurityToken = await CreateTokenAsync(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            // Generate Refresh Token
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            // Set Refresh Token in Cookie
            SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);

            return new BaseResponse<UserResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "Email verified successfully. You are now logged in.",
                Data = new UserResponse
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = accessToken,
                    RefreshToken = refreshToken.Token,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    RefreshTokenExpiration = refreshToken.ExpiresOn
                }
            };
        }

        #endregion

    }
}
