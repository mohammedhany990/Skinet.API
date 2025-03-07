using Microsoft.AspNetCore.Identity;
using Skinet.API.ExtensionMethods;
using Skinet.Core.Entities.Identity;
using Skinet.Service.Abstracts;

namespace Skinet.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Address> GetUserAddressAsync(string email)
        {
            var user = await _userManager.FindUserWithAddressAsync(email);
            return user.Address;
        }


        public async Task<string> UpdateAddressAsync(Address address, string email)
        {

            var user = await _userManager.FindUserWithAddressAsync(email);

            address.Id = user.Address?.Id ?? 0;

            user.Address = address;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return "Failed to update address.";
            }

            return "Address updated successfully.";

        }
    }
}
