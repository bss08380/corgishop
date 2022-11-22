namespace CorgiShop.Application.Caching;

public record CacheResponse<X>(bool Success, X? CachedObject)
{
    public bool IsValid => Success && CachedObject != null;
}
