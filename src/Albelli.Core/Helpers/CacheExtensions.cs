using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Albelli.Core.Helpers;

public static class CacheExtensions
{
    public static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions();

        if (expirationTime.HasValue)
        {
            options.SetAbsoluteExpiration(expirationTime.Value);
        }

        return distributedCache.SetStringAsync(key, JsonSerializer.Serialize(entry), options, cancellationToken);
    }

    public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken = default)
    {
        var stored = await distributedCache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(stored))
        {
            return JsonSerializer.Deserialize<T>(stored);
        }

        return default;
    }

    public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> func, TimeSpan expirationTime)
    {
        TItem result = await cache.GetAsync<TItem>(key);

        if (result == null)
        {
            result = await func();
            await cache.SetAsync(key, result, expirationTime);
        }

        return result;
    }
}