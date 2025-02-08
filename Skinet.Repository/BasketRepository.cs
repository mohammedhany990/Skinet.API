using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Interfaces;
using StackExchange.Redis;

namespace Skinet.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            var data  = basket.IsNullOrEmpty ?
                null : JsonSerializer.Deserialize<CustomerBasket>(basket);

            return data;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var newBasket = await
                _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(15));

            return !newBasket ? null : await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}
