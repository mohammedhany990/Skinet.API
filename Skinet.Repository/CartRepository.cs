using Skinet.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities.Cart;
using Newtonsoft.Json;
using Skinet.Core.Entities;

namespace Skinet.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabase _database;

        public CartRepository(IConnectionMultiplexer redis, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _database = redis.GetDatabase();
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cartJson = await _database.StringGetAsync($"cart:{userId}");
            return cartJson.IsNullOrEmpty ?
                null : JsonConvert.DeserializeObject<Cart>(cartJson);
        }

        public async Task<string> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId) ??
                       new Cart(Guid.NewGuid().ToString(), userId);

            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            if (product is null) return "Product not found";

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem(productId, product.Name, product.Price, quantity);
                cart.Items.Add(cartItem);
            }

            cart.CalculateTotal();
            await _database.StringSetAsync($"cart:{userId}", JsonConvert.SerializeObject(cart), TimeSpan.FromDays(15));

            return "Item added successfully";
        }

        public async Task<string> RemoveItemFromCartAsync(string userId, string cartItemId)
        {
            var cart = await GetCartAsync(userId);
            if (cart is null) return "Cart not found";

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return "Item not found";

            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
            }
            else
            {
                cart.Items.Remove(item);
            }

            cart.CalculateTotal();
            await _database.StringSetAsync($"cart:{userId}", JsonConvert.SerializeObject(cart));

            return "Item updated successfully";
        }

        // Can you use too to remove all Items by setting quantity = 0
        public async Task<string> UpdateItemQuantityAsync(string userId, string cartItemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            if (cart is null) return "Cart not found";

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return "Item not found";

            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            cart.CalculateTotal();
            await _database.StringSetAsync($"cart:{userId}", JsonConvert.SerializeObject(cart));

            return quantity > 0 ? "Item quantity updated successfully" : "Item removed from cart";
        }

        public async Task<string> ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            if (cart is null) return "Cart not found";

            cart.Items.Clear();
            cart.CalculateTotal();

            await _database.StringSetAsync($"cart:{userId}", JsonConvert.SerializeObject(cart));
            return "Cart cleared successfully";
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            return cart?.Total ?? 0;
        }
    }
}
