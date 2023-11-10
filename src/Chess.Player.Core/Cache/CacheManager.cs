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

    public TimeSpan? GetCacheInvalidatePeriod(bool useCache)
    {
        return useCache
            ? null
            : TimeSpan.FromMinutes(1);
    }

    public Task<T?> GetAsync<T>(string key, TimeSpan? invalidatePeriod, CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<ICache<T>>().GetAsync(key, invalidatePeriod, cancellationToken);
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