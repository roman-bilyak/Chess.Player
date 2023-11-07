namespace Chess.Player.Cache;

public interface ICache<T>
{
    Task<T?> GetAsync(string key, CancellationToken cancellationToken);

    Task AddAsync(string key, T value, CancellationToken cancellationToken);

    Task ClearAsync(CancellationToken cancellationToken);
}