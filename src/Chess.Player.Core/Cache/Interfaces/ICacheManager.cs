namespace Chess.Player.Cache;

public interface ICacheManager
{
    Task<T?> GetAsync<T>(string key, bool includeExpired, CancellationToken cancellationToken);

    Task AddAsync<T>(string key, T value, DateTime? expirationDate, CancellationToken cancellationToken);

    Task ClearAllAsync<T>(CancellationToken cancellationToken);
}