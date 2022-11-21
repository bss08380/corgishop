namespace CorgiShop.Application.Abstractions;

public interface ICacheableQuery
{
    bool CacheEnable { get; }
    string CacheKey { get; }
    TimeSpan? TimeToLive { get; }
}
