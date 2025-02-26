namespace Skinet.Core.DTOs.Identity
{
    public class UserWithRoleResponse
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
