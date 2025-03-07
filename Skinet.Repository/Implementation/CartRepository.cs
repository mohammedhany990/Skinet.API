using Skinet.Core.Entities;
using Skinet.Core.Entities.Cart;
using Skinet.Core.Specifications;
using Skinet.Repository.Interfaces;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Skinet.Repository.Implementation
{
    public class CartRepository : ICartRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabase _database;
        private static readonly TimeSpan CartExpiration = TimeSpan.FromDays(15);

        public CartRepository(IConnectionMultiplexer redis, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _database = redis.GetDatabase();
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cartJson = await _database.StringGetAsync($"cart:{userId}");
            return cartJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cartJson);
        }



        public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId) ?? new Cart(userId);

            var spec = new ProductWithBrandAndTypeSpecification(productId);
            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);

            if (product is null) return false;

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem(productId, product.Name, product.Price, quantity, product.ProductBrand.Name,
                    product.ProductType.Name, product.PictureUrl));
            }

            cart.CalculateTotal();
            await SaveCartAsync(userId, cart);

            return true;
        }

        public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await GetCartAsync(userId);

            if (cart == null) return false;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return false;

            cart.Items.Remove(item);

            cart.CalculateTotal();

            await SaveCartAsync(userId, cart);
            return true;
        }


        public async Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            if (cart is null) return false;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item is null) return false;

            if (quantity > 0)
                item.Quantity = quantity;
            else
                cart.Items.Remove(item);

            cart.CalculateTotal();
            await SaveCartAsync(userId, cart);
            return true;
        }


        public async Task<bool> ClearCartAsync(string userId)
        {
            return await _database.KeyDeleteAsync($"cart:{userId}");
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            if (cart is null) return 0;
            cart.CalculateTotal();
            return cart?.Total ?? 0;
        }

        public async Task<bool> UpdateCartAsync(string userId, Cart cart)
        {
            await SaveCartAsync(userId, cart);
            return true;
        }


        private async Task SaveCartAsync(string userId, Cart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            await _database.StringSetAsync($"cart:{userId}", cartJson, CartExpiration);
        }
    }
}