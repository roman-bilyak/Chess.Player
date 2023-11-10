using Chess.Player.Cache;
using Chess.Player.Data;

namespace Chess.Player.MAUI.Cache;

internal class CacheDataFileCache<T> : FileCache<T>
{
    public CacheDataFileCache(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
    }

    protected override string RootPath => FileSystem.Current.CacheDirectory;
}