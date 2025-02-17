using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Skinet.API.Errors;
using Skinet.API.ExtensionMethods;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Skinet.API.Controllers
{

    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return NotFound(new ApiResponse(404));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var jwtSecurityToken = await _tokenService.CreateTokenAsync(user, _userManager);

            var returnedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
            return Ok(returnedUser);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckExisting(registerDto.Email).Result)
            {
                return BadRequest(new ApiResponse(400, "This Account Already Exists"));
            }
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ValidationErrorResponse()
                {
                    Errors = result.Errors.Select(E => E.Description)
                });
            }

            var jwtSecurityToken = await _tokenService.CreateTokenAsync(user, _userManager);

            var returnedUser = new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };


            return Ok(returnedUser);

        }
        #region DeleteAccount

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteAccount()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                var res = await _userManager.DeleteAsync(user);
                if (res.Succeeded)
                {
                    return Ok(new ApiResponse(200, "Deleted Successfully!"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Error deleting data."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(400, "User not found"));
            }
        }
        #endregion

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var jwtSecurityToken = await _tokenService.CreateTokenAsync(user, _userManager);


            var returnedUser = new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),


            };
            return Ok(returnedUser);
        }

        [HttpGet("get-user-address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var ReturnesAddress = _mapper.Map<Address, AddressDto>(user.Address);
            if (ReturnesAddress is null)
            {
                return NotFound(new ApiResponse(404, "No Address Found"));
            }

            return Ok(ReturnesAddress);
        }

        #region UpdateAddress
        [Authorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<Address>> UpdateAddress(AddressDto dto)
        {
            var address = _mapper.Map<AddressDto, Address>(dto);

            var user = await _userManager.FindUserWithAddressAsync(User);

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


        [HttpGet("email-exists")]
        public async Task<bool> CheckExisting(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }
    }
}
