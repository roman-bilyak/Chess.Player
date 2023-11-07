namespace Chess.Player.Cache;

public interface ICacheManager
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

    Task AddAsync<T>(string key, T value, CancellationToken cancellationToken);

    Task ClearAsync<T>(CancellationToken cancellationToken);
}