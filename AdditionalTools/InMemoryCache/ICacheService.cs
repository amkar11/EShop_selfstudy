using System.Runtime.CompilerServices;

namespace AdditionalTools.InMemoryCache;

public interface ICacheService
{
    Task<T> GetOrAddValueAsync<T>(string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    void SetRefreshTokenAndTimeoutToCache(string key, string refresh_token);

    void DeleteValue(string key);

    string? GetRefreshTokenFromCache(string key);
}
