using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Skinet.API.Errors;
using Skinet.API.ExtensionMethods;
using Skinet.API.Features.Authentication.Commands.Delete;
using Skinet.API.Features.Authentication.Commands.ForgotPassword;
using Skinet.API.Features.Authentication.Commands.SendOtpAsync;
using Skinet.API.Features.Authentication.Commands.VerifyOtp;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Helper;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _tokenService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService tokenService,
            IMapper mapper,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<UserResponse>>> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<UserResponse>>> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [Authorize]
        [MapToApiVersion("1.0")]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteAccount()
        {
            var response = await _mediator.Send(new DeleteCommand());
            return Ok(response);
        }
        /*
        [Authorize]
        [MapToApiVersion("1.0")]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserResponse>> GetCurrentUserAsync()
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var jwtSecurityToken = await _tokenService.CreateTokenAsync(user, _userManager);


            var returnedUser = new UserResponse()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
            return Ok(returnedUser);
        }
        */
        [HttpGet("get-user-address")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult<AddressModel>> GetUserAddressAsync()
        {
            var user = await _userManager.FindUserWithAddressAsync("User");

            var ReturnesAddress = _mapper.Map<Address, AddressModel>(user.Address);
            if (ReturnesAddress is null)
            {
                return NotFound(new ApiResponse(404, "No Address Found"));
            }

            return Ok(ReturnesAddress);
        }


        #region UpdateAddress
        [Authorize]
        [MapToApiVersion("1.0")]
        [HttpPut("update-address")]
        public async Task<ActionResult<Address>> UpdateAddress(AddressModel dto)
        {
            var address = _mapper.Map<AddressModel, Address>(dto);

            var user = await _userManager.FindUserWithAddressAsync("User");

            address.Id = user.Address.Id;

            user.Address = address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(dto);
        }
        #endregion



        [MapToApiVersion("1.0")]
        [HttpPost("send-otp")]
        public async Task<ActionResult> SndOtp(SendOtpCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifySignupOtp(VerifyOtpCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> ChangePassword(ChangePasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<BaseResponse<string>>> ForgotPassword(ForgotPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("reset-password")]
        public async Task<ActionResult<BaseResponse<string>>> ResetPasswordAsync(ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
