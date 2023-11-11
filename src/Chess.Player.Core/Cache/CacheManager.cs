using Microsoft.Extensions.DependencyInjection;

namespace Chess.Player.Cache;

internal class CacheManager : ICacheManager
{
    private readonly IServiceProvider _serviceProvider;

    public CacheManager
    (
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _serviceProvider = serviceProvider;
    }

    public Task<T?> GetAsync<T>(string key, bool includeExpired, CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().GetAsync(key, includeExpired, cancellationToken);
    }

    public Task AddAsync<T>(string key, T value, DateTime? expirationDate, CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().AddAsync(key, value, expirationDate, cancellationToken);
    }

    public Task ClearAllAsync<T>(CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().ClearAllAsync(cancellationToken);
    }
}