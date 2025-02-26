using Skinet.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Skinet.Service.Implementation
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object value, TimeSpan expiryTime)
        {
            if(value is null)
                return;

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedValue = JsonSerializer.Serialize(value, options);

            await _database.StringSetAsync(cacheKey, serializedValue, expiryTime);
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var response = await _database.StringGetAsync(cacheKey);

            if (response.IsNullOrEmpty)
                return null;

            return response;
        }
    }
}
