namespace Chess.Player.Cache;

public static class ICacheManagerExtensions
{
    private const string DefaultKey = "Default";

    public static Task<T?> GetAsync<T>(this ICacheManager cacheManager, bool includeExpired, CancellationToken cancellationToken)
    {
        return cacheManager.GetAsync<T>(DefaultKey, includeExpired, cancellationToken);
    }

    public static Task AddAsync<T>(this ICacheManager cacheManager, T value, DateTime? expirationDate, CancellationToken cancellationToken)
    {
        return cacheManager.AddAsync<T>(DefaultKey, value, expirationDate, cancellationToken);
    }

    public static async Task<T> GetOrAddAsync<T>(this ICacheManager cacheManager, string key, bool includeExpired, Func<Task<T>> valueFactory, Func<T, DateTime?> getExpirationDate, CancellationToken cancellationToken)
    {
        T? cacheItem = await cacheManager.GetAsync<T>(key, includeExpired, cancellationToken);
        if (cacheItem is not null)
        {
            return cacheItem;
        }

        T value = await valueFactory();
        await cacheManager.AddAsync(key, value, getExpirationDate(value), cancellationToken);

        return value;
    }
}