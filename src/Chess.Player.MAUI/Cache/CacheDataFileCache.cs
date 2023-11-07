using Chess.Player.Cache;

namespace Chess.Player.MAUI.Cache;

internal class CacheDataFileCache<T> : FileCache<T>
{
    protected override string RootPath => FileSystem.Current.CacheDirectory;
}