using Skinet.Core.DTOs.Identity;

namespace Skinet.Service.Abstracts
{
    public interface IAuthorizationServices
    {
        Task<List<UserWithRoleResponse>> GetAllUsersAsync();


        Task<string> AssignRoleToUserAsync(string userId, string roleName);

        Task<List<RoleResponse>> GetUserRolesAsync(string userId);

        Task<string> RemoveUserRoleAsync(string userId, string roleName);

        Task<bool> IsRoleExistsAsync(string roleName);

        Task<List<RoleResponse>> GetRolesAsync();

        Task<RoleResponse> GetRoleByIdAsync(string id);

        Task<string> CreateRoleAsync(string roleName);

        Task<string> DeleteRoleAsync(string roleName);

    }
}
