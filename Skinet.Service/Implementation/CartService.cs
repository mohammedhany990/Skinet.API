using Skinet.Core.Entities.Cart;
using Skinet.Repository.Interfaces;
using Skinet.Service.Interfaces;

namespace Skinet.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _unitOfWork.CartRepository.GetCartAsync(userId);
        }

        public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            return await _unitOfWork.CartRepository.AddItemToCartAsync(userId, productId, quantity);
        }

        public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
        {
            return await _unitOfWork.CartRepository.RemoveItemFromCartAsync(userId, productId);
        }

        public async Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantity)
        {
            return await _unitOfWork.CartRepository.UpdateItemQuantityAsync(userId, productId, quantity);
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            return await _unitOfWork.CartRepository.ClearCartAsync(userId);
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {

            return await _unitOfWork.CartRepository.GetCartTotalAsync(userId);
        }

        public async Task<bool> UpdateCartAsync(string userId, Cart cart)
        {
            return await _unitOfWork.CartRepository.UpdateCartAsync(userId, cart);
        }
    }
}
