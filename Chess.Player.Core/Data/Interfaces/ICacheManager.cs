namespace Chess.Player.Data;

public interface ICacheManager
{
    Task<T?> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T?>> valueFactory, bool forceRefresh, CancellationToken cancellationToken);

    Task DeleteAsync(string cacheType, string key, CancellationToken cancellationToken);

    Task DeleteAsync(string cacheType, CancellationToken cancellationToken);

    Task DeleteAsync(CancellationToken cancellationToken);
}