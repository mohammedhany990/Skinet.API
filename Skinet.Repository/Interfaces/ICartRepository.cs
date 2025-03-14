﻿using Skinet.Core.Entities.Cart;

namespace Skinet.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId);
        Task<bool> AddItemToCartAsync(string userId, int productId, int quantity);
        Task<bool> RemoveItemFromCartAsync(string userId, int productId);
        Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantity);
        Task<bool> ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task<bool> UpdateCartAsync(string userId, Cart cart);
    }
}