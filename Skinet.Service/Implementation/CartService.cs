using Skinet.Core.Entities.Cart;
using Skinet.Core.Interfaces;
using Skinet.Service.Interfaces;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Skinet.Service.Implementation
{
    class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            // Fetches the cart using the CartRepository
            return await _unitOfWork.CartRepository.GetCartAsync(userId);
        }

        public async Task<string> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            // Uses the CartRepository to add an item to the cart
            return await _unitOfWork.CartRepository.AddItemToCartAsync(userId, productId, quantity);
        }

        public async Task<string> RemoveItemFromCartAsync(string userId, string cartItemId)
        {
            // Uses the CartRepository to remove an item from the cart
            return await _unitOfWork.CartRepository.RemoveItemFromCartAsync(userId, cartItemId);
        }

        public async Task<string> UpdateItemQuantityAsync(string userId, string cartItemId, int quantity)
        {
            // Uses the CartRepository to update the quantity of an item in the cart
            return await _unitOfWork.CartRepository.UpdateItemQuantityAsync(userId, cartItemId, quantity);
        }

        public async Task<string> ClearCartAsync(string userId)
        {
            // Uses the CartRepository to clear the cart
            return await _unitOfWork.CartRepository.ClearCartAsync(userId);
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            // Fetches the total price of the cart using CartRepository
            return await _unitOfWork.CartRepository.GetCartTotalAsync(userId);
        }
    }
}
