﻿using Microsoft.AspNetCore.Identity;

namespace Skinet.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }//
}
