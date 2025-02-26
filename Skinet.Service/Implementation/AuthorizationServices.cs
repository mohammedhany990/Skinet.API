using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.DTOs.Identity;
using Skinet.Core.Entities.Identity;
using Skinet.Service.Abstracts;

namespace Skinet.Service.Implementation
{
    public class AuthorizationServices : IAuthorizationServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationServices(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserWithRoleResponse>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserWithRoleResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserWithRoleResponse
                {
                    UserId = user.Id,
                    UserName = user.DisplayName,
                    UserEmail = user.Email,
                    Roles = roles.ToList()
                });
            }

            return userList;
        }


        public async Task<string> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found.";

            if (!await _roleManager.RoleExistsAsync(roleName)) return "Role does not exist.";

            if (await _userManager.IsInRoleAsync(user, roleName)) return "User already has this role.";

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? "Role assigned successfully." : $"Failed to assign role. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}";
        }

        public async Task<List<RoleResponse>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roleNames = await _userManager.GetRolesAsync(user);
            return roleNames.Select(roleName => new RoleResponse { RoleName = roleName }).ToList();
        }

        public async Task<string> RemoveUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found.";
            if (!await _userManager.IsInRoleAsync(user, roleName)) return "User is not in this role.";

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded ? "Role removed successfully." : "Failed to remove role.";
        }



        public async Task<bool> IsRoleExistsAsync(string roleName) => await _roleManager.RoleExistsAsync(roleName);

        public async Task<List<RoleResponse>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => new RoleResponse { RoleId = r.Id, RoleName = r.Name })
                .ToListAsync();
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return role == null ? null : new RoleResponse { RoleId = role.Id, RoleName = role.Name };
        }

        public async Task<string> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName)) return "Role already exists.";

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded ? "Role created successfully." : $"Failed to create role. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}";
        }

        public async Task<string> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return "Role not found.";

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? "Role deleted successfully." : $"Failed to delete role. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}";
        }
    }
}
