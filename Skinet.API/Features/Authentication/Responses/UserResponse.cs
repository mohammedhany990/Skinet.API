using Newtonsoft.Json;

namespace Skinet.Core.Features.Authentication.Responses
{
    public class UserResponse
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public DateTime? ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
