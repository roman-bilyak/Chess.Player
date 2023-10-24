using Newtonsoft.Json;

namespace Chess.Player.Data;

public abstract class FileCacheManager : ICacheManager
{
    protected abstract string RootPath { get; }

    public async Task<T> GetOrAddAsync<T>(string cacheType, string key, Func<Task<T>> valueFactory, bool forceRefresh, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(cacheType, key);
        if (!forceRefresh)
        {
            if (File.Exists(cacheFilePath))
            {
                string json = File.ReadAllText(cacheFilePath);
                T? result = JsonConvert.DeserializeObject<T>(json);
                if (result is null)
                {
                    throw new Exception("Failed to deserialize");
                }
                return result;
            }
        }

        T value = await valueFactory();

        string jsonToWrite = JsonConvert.SerializeObject(value);
        string? cacheFileDirectory = Path.GetDirectoryName(cacheFilePath);
        if (cacheFileDirectory is not null)
        {
            Directory.CreateDirectory(cacheFileDirectory);
        }

        await File.WriteAllTextAsync(cacheFilePath, jsonToWrite, cancellationToken);
        return value;
    }

    public Task DeleteAsync(string cacheType, string key, CancellationToken cancellationToken)
    {
        string cacheFilePath = GetCacheFilePath(key, cacheType);

        if (File.Exists(cacheFilePath))
        {
            File.Delete(cacheFilePath);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string cacheType, CancellationToken cancellationToken)
    {
        string cacheFolderPath = GetCacheFolderPath(cacheType);
        DeleteFiles(cacheFolderPath);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(CancellationToken cancellationToken)
    {
        string cacheFolderPath = GetCacheFolderPath();
        DeleteFiles(cacheFolderPath);

        return Task.CompletedTask;
    }

    #region helper methods

    private string GetCacheFolderPath(string? cacheType = null)
    {
        string? result = RootPath;
        if (cacheType is null)
        {
            return result;
        }
        return Path.Combine(result, cacheType);
    }

    private string GetCacheFilePath(string cacheType, string key)
    {
        string cacheFolderPath = GetCacheFolderPath(cacheType);
        return Path.Combine(cacheFolderPath, $"{key}.json");
    }

    private void DeleteFiles(string folder)
    {
        DirectoryInfo directory = new(folder);
        foreach (FileInfo file in directory.GetFiles("*.json", SearchOption.AllDirectories))
        {
            file.Delete();
        }
    }

    #endregion
}