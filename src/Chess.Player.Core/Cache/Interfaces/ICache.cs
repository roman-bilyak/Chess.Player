namespace Chess.Player.Cache;

public interface ICache<T>
{
    Task<T?> GetAsync(string key, bool includeExpired, CancellationToken cancellationToken);

    Task AddAsync(string key, T value, DateTime? expirationDate, CancellationToken cancellationToken);

    Task ClearAllAsync(CancellationToken cancellationToken);
}