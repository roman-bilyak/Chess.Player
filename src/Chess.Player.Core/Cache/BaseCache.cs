using Chess.Player.Cache.Interfaces;
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

    protected abstract Task<T?> GetAsync(string key, CancellationToken cancellationToken);

    public async Task<T?> GetAsync(string key, TimeSpan? invalidatePeriod, CancellationToken cancellationToken)
    {
        T? value = await GetAsync(key, cancellationToken);

        if (ShouldInvalidate(value, invalidatePeriod))
        {
            return default;
        }

        return value;
    }

    protected abstract Task StoreAsync(string key, T value, CancellationToken cancellationToken);

    public async Task AddAsync(string key, T value, CancellationToken cancellationToken)
    {
        if (value is ICacheItem cacheItem
            && cacheItem.LastUpdateTime is null)
        {
            cacheItem.LastUpdateTime = _dateTimeProvider.UtcNow;
        }

        await StoreAsync(key, value, cancellationToken);
    }

    public abstract Task ClearAsync(CancellationToken cancellationToken);

    #region helper methods

    private bool ShouldInvalidate(T? value, TimeSpan? invalidatePeriod)
    {
        return value is not null
            && invalidatePeriod is not null
            && value is ICacheItem cacheItem
            && cacheItem.LastUpdateTime is not null
            && (_dateTimeProvider.UtcNow - cacheItem.LastUpdateTime) > invalidatePeriod;
    }

    #endregion
}