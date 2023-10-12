namespace Chess.Player.Data
{
    internal interface ICacheManager
    {
        Task<T?> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T?>> valueFactory);

        Task DeleteAsync(string cacheType, string key);

        Task DeleteAsync(string cacheType);

        Task DeleteAsync();
    }
}