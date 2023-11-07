using Newtonsoft.Json;

namespace Chess.Player.Cache;

public class FileCache<T> : ICache<T>
{
    protected virtual string RootPath => "Cache";

    protected virtual string CacheName => typeof(T).Name;

    public async Task<T?> GetAsync(string key, CancellationToken cancellationToken)
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

    public async Task AddAsync(string key, T value, CancellationToken cancellationToken)
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

    public Task ClearAsync(CancellationToken cancellationToken)
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