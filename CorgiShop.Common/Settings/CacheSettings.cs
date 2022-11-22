namespace CorgiShop.Common.Settings;

public record CacheSettings
{
    public required int MinutesToLive { get; init; }
}
