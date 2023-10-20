namespace Chess.Player.Data
{
    public interface ICacheManager
    {
        Task<T?> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T?>> valueFactory, bool forceRefresh = false);

        Task DeleteAsync(string cacheType, string key);

        Task DeleteAsync(string cacheType);

        Task DeleteAsync();
    }
}