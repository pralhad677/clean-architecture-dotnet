using System.Buffers;
using System.Text.Json;
using Bookify.Application.Abstraction.Caching;
using Microsoft.Extensions.Caching.Distributed;
// using Newtonsoft.Json;
using Utf8JsonWriter = System.Text.Json.Utf8JsonWriter;

namespace Bookify.Infrastructure.Caching;

internal sealed class CacheService:ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache distributedCache)
    {
        _cache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
      byte[]? bytes = await _cache.GetAsync(key, cancellationToken);
      return bytes is null ? default : Deserialize<T>(bytes);
    }



    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);
        return _cache.SetAsync(key, bytes, CacheOptions.Create(expiration ), cancellationToken);
    }



    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
    return _cache.RemoveAsync(key, cancellationToken);
    }
    private static T Deserialize<T>(byte[] bytes)
    {
         return JsonSerializer.Deserialize<T>(bytes);
    }
    private byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }
}