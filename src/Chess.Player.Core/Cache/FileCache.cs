using Chess.Player.Data;
using Newtonsoft.Json;

namespace Chess.Player.Cache;

public class FileCache<T> : BaseCache<T>
{
    protected virtual string RootPath => "Cache";

    public FileCache(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
    }

    protected sealed override async Task<CacheItem<T>?> GetCacheItemAsync(string key, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(key);

        if (!File.Exists(cacheFilePath))
        {
            return default;
        }

        string json = await File.ReadAllTextAsync(cacheFilePath, cancellationToken);
        CacheItem<T>? cacheItem = JsonConvert.DeserializeObject<CacheItem<T>>(json, new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        });

        return cacheItem is null ? throw new Exception("Failed to deserialize") : cacheItem;
    }

    protected sealed override async Task StoreCacheItemAsync(string key, CacheItem<T> cacheItem, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(key);

        string jsonToWrite = JsonConvert.SerializeObject(cacheItem, Formatting.None, new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        });
        string? cacheFileDirectory = Path.GetDirectoryName(cacheFilePath);
        if (cacheFileDirectory is not null)
        {
            Directory.CreateDirectory(cacheFileDirectory);
        }

        await File.WriteAllTextAsync(cacheFilePath, jsonToWrite, cancellationToken);
    }

    public sealed override Task ClearAllAsync(CancellationToken cancellationToken)
    {
        string cacheFolderPath = GetCacheFolderPath();

        DirectoryInfo directory = new(cacheFolderPath);
        if (!directory.Exists)
        {
            return Task.CompletedTask;
        }

        foreach (FileInfo file in directory.GetFiles("*.json", SearchOption.AllDirectories))
        {
            file.Delete();
        }

        return Task.CompletedTask;
    }

    #region helper methods

    private string GetCacheFolderPath()
    {
        return Path.Combine(RootPath, CacheName);
    }

    private string GetCacheFilePath(string key)
    {
        string cacheFolderPath = GetCacheFolderPath();
        return Path.Combine(cacheFolderPath, $"{key}.json");
    }

    #endregion
}