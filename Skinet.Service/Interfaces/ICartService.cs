using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities.Cart;

namespace Skinet.Service.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId); // Get the user's cart
        Task<string> AddItemToCartAsync(string userId, int productId, int quantity); // Add an item to the cart
        Task<string> RemoveItemFromCartAsync(string userId, string cartItemId); // Remove an item from the cart
        Task<string> UpdateItemQuantityAsync(string userId, string cartItemId, int quantity); // Update the quantity of a cart item
        Task<string> ClearCartAsync(string userId); // Clear all items from the cart
        Task<decimal> GetCartTotalAsync(string userId); // Get the total price of the cart
    }


}
