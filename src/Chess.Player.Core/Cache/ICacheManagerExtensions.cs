using Chess.Player.Cache.Interfaces;

namespace Chess.Player.Cache;

public static class ICacheManagerExtensions
{
    private static readonly string DefaultKey = "Default";

    public static Task<T?> GetAsync<T>(this ICacheManager cacheManager, CancellationToken cancellationToken)
    {
        return cacheManager.GetAsync<T>(DefaultKey, cancellationToken);
    }

    public static Task<T?> GetAsync<T>(this ICacheManager cacheManager, string key, CancellationToken cancellationToken)
    {
        return cacheManager.GetAsync<T>(key, null, cancellationToken);
    }

    public static Task AddAsync<T>(this ICacheManager cacheManager, T value, CancellationToken cancellationToken)
    {
        return cacheManager.AddAsync<T>(DefaultKey, value, cancellationToken);
    }

    public static async Task<T> GetOrAddAsync<T>(this ICacheManager cacheManager, string key, TimeSpan? invalidatePeriod, Func<Task<T>> valueFactory, CancellationToken cancellationToken)
        where T : ICacheItem
    {
        T? cacheItem = await cacheManager.GetAsync<T>(key, invalidatePeriod, cancellationToken);
        if (cacheItem is not null)
        {
            return cacheItem;
        }

        T value = await valueFactory();

        await cacheManager.AddAsync(key, value, cancellationToken);

        return value;
    }
}