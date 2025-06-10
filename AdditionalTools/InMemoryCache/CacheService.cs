using Microsoft.Extensions.Caching.Memory;
namespace AdditionalTools.InMemoryCache;

public class CacheService : ICacheService
{
    public readonly IMemoryCache _memoryCache;
    private TimeSpan RefreshTokenTimeout { get; set; } = TimeSpan.FromDays(14);

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public async Task<T> GetOrAddValueAsync<T>(string key, Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        if (_memoryCache.TryGetValue(key, out T? cached) && cached is not null)
        {
            return cached;
        }

        T value = await factory();
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration ?? TimeSpan.FromHours(1))
            .SetSlidingExpiration(slidingExpiration ?? TimeSpan.FromMinutes(1));
        _memoryCache.Set(key, value, options);
        return value;
        
    }
    public void SetRefreshTokenAndTimeoutToCache(string key, string refresh_token)
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(RefreshTokenTimeout)
            .SetSlidingExpiration(RefreshTokenTimeout);

        _memoryCache.Set(key, refresh_token, options);
    }

    public string? GetRefreshTokenFromCache(string key)
    {
        if (_memoryCache.TryGetValue(key, out string? refresh_token) && refresh_token is not null)
        {
            return refresh_token;
        }
        return null;
    }

    public void DeleteValue(string key)
    {
        if (_memoryCache.TryGetValue(key, out _))
        {
            _memoryCache.Remove(key);
        }
    }

   
}
