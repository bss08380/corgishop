using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CorgiShop.Application.Caching;

public class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;

    public CachingService(
        IDistributedCache cache,
        ILogger<CachingService> logger)
    {
        _cache= cache;
        _logger= logger;
    }

    public async Task<X> GetOrRefresh<X>(string cacheKey, Func<Task<X>> getAction)
    {
        var cachedObj = await GetObjectFromCache<X?>(cacheKey);
        if (cachedObj != null && cachedObj.IsValid) return cachedObj.CachedObject!;

        var newObj = await getAction() ?? throw new Exception("Could not retrieve a fresh object value during cache exchange");
        await SaveObjectToCache(cacheKey, newObj);
        return newObj;
    }

    public async Task SaveObjectToCache(string key, object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var bytes = Encoding.ASCII.GetBytes(json);
        await _cache.SetAsync(key, bytes);
    }

    public async Task<CacheResponse<X>> GetObjectFromCache<X>(string key)
    {
        var cachedStr = await _cache.GetAsync(key);
        if (cachedStr == null)
        {
            return new CacheResponse<X>(false, default);
        }

        try
        {
            var obj = JsonSerializer.Deserialize<X>(Encoding.ASCII.GetString(cachedStr));
            return new CacheResponse<X>(true, obj);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deserializing cached data: {cachedStr}", ex);
            return new CacheResponse<X>(false, default);
        }
    }

    public async Task ClearCache(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
