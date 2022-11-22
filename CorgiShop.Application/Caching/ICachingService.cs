namespace CorgiShop.Application.Caching;

public interface ICachingService
{
    Task<X> GetOrRefresh<X>(string cacheKey, Func<Task<X>> getAction);

    Task SaveObjectToCache(string key, object obj);
    Task<CacheResponse<X>> GetObjectFromCache<X>(string key);

    Task ClearCache(string key);
}
