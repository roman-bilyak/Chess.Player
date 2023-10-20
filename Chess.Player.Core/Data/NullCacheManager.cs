namespace Chess.Player.Data
{
    public class NullCacheManager : ICacheManager
    {
        public async Task<T?> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T?>> valueFactory, bool forceRefresh)
        {
            return await valueFactory();
        }

        public Task DeleteAsync(string cacheType, string key)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string cacheType)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync()
        {
            return Task.CompletedTask;
        }
    }
}