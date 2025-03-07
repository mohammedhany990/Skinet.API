using Skinet.Service.Abstracts;
using StackExchange.Redis;
using System.Text.Json;

namespace Skinet.Service.Implementation
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string key, object? value, TimeSpan expire)
        {
            if (value is null)
            {
                return;
            }
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serializedValue = JsonSerializer.Serialize(value, options);
            await _database.StringSetAsync(key, serializedValue, expire);
        }

        public async Task<string?> GetCacheResponseAsync(string key)
        {
            var res = await _database.StringGetAsync(key);
            if (res.IsNullOrEmpty)
            {
                return null;
            }
            return res;
        }
    }
}
