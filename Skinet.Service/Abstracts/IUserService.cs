using Address = Skinet.Core.Entities.Identity.Address;


namespace Skinet.Service.Abstracts
{
    public interface IUserService
    {
        Task<Address> GetUserAddressAsync(string email);
        Task<string> UpdateAddressAsync(Address address, string email);
    }
}
