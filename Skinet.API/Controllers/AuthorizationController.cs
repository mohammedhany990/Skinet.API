using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Helper;
using Skinet.Service.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class AuthorizationController : ApiBaseController
    {
        private readonly IAuthService _authService;
        private readonly IAuthorizationServices _authorizationService;
        private readonly UserManager<AppUser> _userManager;

        public AuthorizationController(IAuthService authService,IAuthorizationServices authorizationService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [ApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<ActionResult<BaseResponse<string>>> AssignRoleToUser([FromQuery, Required] string userId, [FromQuery, Required] string roleName)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return Ok(new BaseResponse<string>(400, false, "User ID and Role Name are required."));
            }

            var currentUserId = GetUserId();
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Ok(new BaseResponse<string>(401, false, "User is not authenticated."));
            }

            if (await _authService.IsUserExistsByIdAsync(userId) == false)
            {
                return Ok(new BaseResponse<string>(404, false, "user not found."));
            }

            if (!await _authorizationService.IsRoleExistsAsync(roleName))
            {
                return Ok(new BaseResponse<string>(404, false, "Role not found,check role existing before adding."));
            }

            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin)
            {
                return Ok(new BaseResponse<string>(403, false, "Only admins can assign roles."));
            }

            var result = await _authorizationService.AssignRoleToUserAsync(userId, roleName);
            if (result == "Role assigned successfully.")
            {
                return Ok(new BaseResponse<string>(200, true, "Role assigned successfully."));
            }
            return Ok(new BaseResponse<string>(400, false, result));
        }

        [ApiVersion("1.0")]
        [HttpGet("user-roles/{userId}")]
        public async Task<ActionResult<BaseResponse<List<RoleResponse>>>> GetUserRoles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(new BaseResponse<List<RoleResponse>>(400, false, "User ID is required."));
            }

            var roles = await _authorizationService.GetUserRolesAsync(userId);

            if (roles is null)
            {
                return NotFound(new BaseResponse<List<RoleResponse>>(404, false, "User not found."));
            }
            return Ok(new BaseResponse<List<RoleResponse>>(200, true, roles.Count, roles, "Roles retrieved successfully."));
        }

        [ApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<ActionResult<BaseResponse<List<UserWithRoleResponse>>>> GetAllUsersAsync()
        {
            var result = await _authorizationService.GetAllUsersAsync();

            if (result is null || !result.Any())
            {
                return Ok(new BaseResponse<List<UserWithRoleResponse>>(404, false, "No users found."));
            }

            return Ok(new BaseResponse<List<UserWithRoleResponse>>(200, true, result.Count, result, "Users retrieved successfully."));
        }

        [Authorize(Roles = "Admin")]
        [ApiVersion("1.0")]
        [HttpDelete("remove-role")]
        public async Task<ActionResult<BaseResponse<string>>> RemoveUserRole([FromQuery] string userId, [FromQuery] string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return Ok(new BaseResponse<string>(400, false, "User ID and Role Name are required."));
            }

            var result = await _authorizationService.RemoveUserRoleAsync(userId, roleName);
            if (result == "Role removed successfully.")
            {
                return Ok(new BaseResponse<string>(200, true, result));

            }
            return Ok(new BaseResponse<string>(400, false, result));
        }

        [Authorize(Roles = "Admin")]
        [ApiVersion("1.0")]
        [HttpGet("role-exists/{roleName}")]
        public async Task<ActionResult<BaseResponse<string>>> IsRoleExisted(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return Ok(new BaseResponse<string>(400, false, "Role Name is required."));
            }

            if (await _authorizationService.IsRoleExistsAsync(roleName))
            {
                return Ok(new BaseResponse<string>(200, true, "Role existed."));

            }
            return Ok(new BaseResponse<bool>(404, false, "Role not found"));
        }

        [ApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpGet("all-roles")]
        public async Task<ActionResult<BaseResponse<List<RoleResponse>>>> GetAllRoles()
        {
            var roles = await _authorizationService.GetRolesAsync();
            if (roles is null || !roles.Any())
            {
                return Ok(new BaseResponse<List<RoleResponse>>(404, false, "Roles not found"));
            }
            return Ok(new BaseResponse<List<RoleResponse>>(200, true, roles.Count, roles, "Roles retrieved successfully."));
        }

        [ApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpGet("role/{id}")]
        public async Task<ActionResult<BaseResponse<RoleResponse>>> GetRoleById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ok(new BaseResponse<RoleResponse>(400, false, "Role ID is required."));
            }

            var role = await _authorizationService.GetRoleByIdAsync(id);
            if (role is null)
            {
                return Ok(new BaseResponse<RoleResponse>(404, false, "Role not found."));
            }
            return Ok(new BaseResponse<RoleResponse>(200, true, 1, role, "Role retrieved successfully."));
        }

        [ApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [HttpPost("create-role")]
        public async Task<ActionResult<BaseResponse<string>>> CreateRole([FromQuery, Required] string roleName)
        {
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin)
            {
                return Ok(new BaseResponse<string>(403, false, "Only admins can create roles."));
            }

            if (await _authorizationService.IsRoleExistsAsync(roleName))
            {
                return Ok(new BaseResponse<string>(400, false, "Role already exists."));
            }

            var result = await _authorizationService.CreateRoleAsync(roleName);
            if (result == "Role created successfully.")
            {
                return Ok(new BaseResponse<string>(200, true, result));
            }
            return Ok(new BaseResponse<string>(500, false, result));
        }

        [Authorize(Roles = "Admin")]
        [ApiVersion("1.0")]
        [HttpDelete("delete-role")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteRole([FromQuery, Required] string roleName)
        {

            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin)
            {
                return Ok(new BaseResponse<string>(403, false, "Only admins can create roles."));
            }

            if (!await _authorizationService.IsRoleExistsAsync(roleName))
            {
                return Ok(new BaseResponse<string>(400, false, "Role not found."));
            }

            var result = await _authorizationService.DeleteRoleAsync(roleName);
            if (result == "Role deleted successfully.")
            {
                return Ok(new BaseResponse<string>(200, true, result));
            }
            return Ok(new BaseResponse<string>(500, false, result));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}