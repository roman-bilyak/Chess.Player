namespace Chess.Player.Cache;

public interface ICacheManager
{
    TimeSpan GetCacheInvalidatePeriod(bool useCache, bool isArchive);

    Task<T?> GetAsync<T>(string key, TimeSpan? invalidatePeriod, CancellationToken cancellationToken);

    Task AddAsync<T>(string key, T value, CancellationToken cancellationToken);

    Task ClearAsync<T>(CancellationToken cancellationToken);
}