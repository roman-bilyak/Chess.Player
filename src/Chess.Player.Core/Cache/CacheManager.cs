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

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().GetAsync(key, cancellationToken);
    }

    public Task AddAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().AddAsync(key, value, cancellationToken);
    }

    public Task ClearAsync<T>(CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().ClearAsync(cancellationToken);
    }
}