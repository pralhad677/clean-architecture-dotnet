using Microsoft.Extensions.Caching.Distributed;

namespace Bookify.Infrastructure.Caching;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration is null ? new DistributedCacheEntryOptions{AbsoluteExpirationRelativeToNow = expiration} : DefaultExpiration;
}