using Chess.Player.Data;

namespace Chess.Player.Cache;

public abstract class BaseCache<T> : ICache<T>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    protected virtual string CacheName => typeof(T).Name;

    public BaseCache
    (
        IDateTimeProvider dateTimeProvider
    )
    {
        ArgumentNullException.ThrowIfNull(dateTimeProvider);

        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<T?> GetAsync(string key, bool includeExpired, CancellationToken cancellationToken)
    {
        CacheItem<T>? value = await GetCacheItemAsync(key, cancellationToken);

        if (value == null)
        {
            return default;
        }

        if (includeExpired
            || value.ExpirationDate is null
            || value.ExpirationDate > _dateTimeProvider.UtcNow)
        {
            return value.Value;
        }

        return default;
    }

    protected abstract Task<CacheItem<T>?> GetCacheItemAsync(string key, CancellationToken cancellationToken);

    public async Task AddAsync(string key, T value, DateTime? expirationDate, CancellationToken cancellationToken)
    {
       CacheItem<T> cacheItem = new(value, expirationDate);
       await StoreCacheItemAsync(key, cacheItem, cancellationToken);
    }

    protected abstract Task StoreCacheItemAsync(string key, CacheItem<T> cacheItem, CancellationToken cancellationToken);

    public abstract Task ClearAllAsync(CancellationToken cancellationToken);
}