namespace Chess.Player.Data;

internal class NullCacheManager : ICacheManager
{
    public async Task<T> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T>> valueFactory, bool forceRefresh, CancellationToken cancellationToken)
    {
        return await valueFactory();
    }

    public Task DeleteAsync(string cacheType, string key, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string cacheType, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}