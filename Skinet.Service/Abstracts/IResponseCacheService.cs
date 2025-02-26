using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Service.Abstracts
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object value, TimeSpan expiryTime);
        Task<string?> GetCachedResponseAsync(string cacheKey);
    }
}
