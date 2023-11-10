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

    protected sealed override async Task<T?> GetAsync(string key, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(key);

        if (!File.Exists(cacheFilePath))
        {
            return default;
        }

        string json = await File.ReadAllTextAsync(cacheFilePath, cancellationToken);
        T? result = JsonConvert.DeserializeObject<T>(json);

        return result is null ? throw new Exception("Failed to deserialize") : result;
    }

    protected sealed override async Task StoreAsync(string key, T value, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(key);

        string jsonToWrite = JsonConvert.SerializeObject(value);
        string? cacheFileDirectory = Path.GetDirectoryName(cacheFilePath);
        if (cacheFileDirectory is not null)
        {
            Directory.CreateDirectory(cacheFileDirectory);
        }

        await File.WriteAllTextAsync(cacheFilePath, jsonToWrite, cancellationToken);
    }

    public sealed override Task ClearAsync(CancellationToken cancellationToken)
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