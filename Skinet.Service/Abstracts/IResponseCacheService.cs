namespace Skinet.Service.Abstracts
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object value, TimeSpan expiryTime);
        Task<string?> GetCacheResponseAsync(string cacheKey);
    }
}
