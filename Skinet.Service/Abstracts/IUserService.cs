using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Stripe;
using Address = Skinet.Core.Entities.Identity.Address;


namespace Skinet.Service.Abstracts
{
    public interface IUserService
    {
        Task<Address> GetUserAddressAsync(string email);
        Task<string> UpdateAddressAsync(Address address , string email);
    }
}
