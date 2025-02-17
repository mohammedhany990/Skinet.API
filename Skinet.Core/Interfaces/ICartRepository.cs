using Skinet.Core.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId); 
        Task<string> AddItemToCartAsync(string userId, int productId, int quantity);
        Task<string> RemoveItemFromCartAsync(string userId, string cartItemId);
        Task<string> UpdateItemQuantityAsync(string userId, string cartItemId, int quantity); 
        Task<string> ClearCartAsync(string userId); 
        Task<decimal> GetCartTotalAsync(string userId);
    }

}
