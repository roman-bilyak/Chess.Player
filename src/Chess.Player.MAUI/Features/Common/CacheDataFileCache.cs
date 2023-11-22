using Chess.Player.Cache;
using Chess.Player.Data;

namespace Chess.Player.MAUI.Features;

internal class CacheDataFileCache<T> : FileCache<T>
{
    public CacheDataFileCache(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
    }

    protected override string RootPath => FileSystem.Current.CacheDirectory;
}