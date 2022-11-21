using Azure;
using CorgiShop.Application.Abstractions;
using CorgiShop.Common.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace CorgiShop.Application.Middleware;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheableQuery
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _settings;
    private readonly ILogger _logger;

    public CachingBehavior(
        IDistributedCache cache, 
        IOptions<CacheSettings> settings,
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!request.CacheEnable) return await next();
        var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

        TResponse response;
        if (cachedResponse != null)
        {

            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse))!;
            _logger.LogDebug($"Cache Read [{typeof(TRequest).Name}] - Key: {request.CacheKey}");
        }
        else
        {
            response = await FetchFreshAndCache(request, next, cancellationToken);
            _logger.LogDebug($"Cache Write [{typeof(TRequest).Name}] - Key: {request.CacheKey}");
        }
        return response;
    }

    private async Task<TResponse> FetchFreshAndCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var cacheOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = request.TimeToLive ?? TimeSpan.FromMinutes(_settings.MinutesToLive)
        };

        var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response));
        await _cache.SetAsync(request.CacheKey, serializedData, cacheOptions, cancellationToken);
        return response;
    }

}
