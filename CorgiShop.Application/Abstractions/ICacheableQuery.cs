namespace CorgiShop.Application.Abstractions;

public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan? TimeToLive { get; }
}
