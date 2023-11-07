namespace Chess.Player.Cache;

public static class ICacheManagerExtensions
{
    private static readonly string DefaultKey = "Default";

    public static Task<T?> GetAsync<T>(this ICacheManager cacheManager, CancellationToken cancellationToken)
    {
        return cacheManager.GetAsync<T>(DefaultKey, cancellationToken);
    }

    public static Task AddAsync<T>(this ICacheManager cacheManager, T value, CancellationToken cancellationToken)
    {
        return cacheManager.AddAsync<T>(DefaultKey, value, cancellationToken);
    }

    public static Task<T> GetOrAddAsync<T>(this ICacheManager cacheManager, Func<Task<T>> valueFactory, bool forceRefresh, CancellationToken cancellationToken)
    {
        return cacheManager.GetOrAddAsync(DefaultKey, valueFactory, forceRefresh, cancellationToken);
    }

    public static async Task<T> GetOrAddAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> valueFactory, bool forceRefresh, CancellationToken cancellationToken)
    {
        if (!forceRefresh)
        {
            T? result = await cacheManager.GetAsync<T>(key, cancellationToken);
            if (result is not null)
            {
                return result;
            }
        }

        T value = await valueFactory();

        await cacheManager.AddAsync(key, value, cancellationToken);

        return value;
    }
}