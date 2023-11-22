using Chess.Player.Cache;
using Chess.Player.Data;

namespace Chess.Player.MAUI.Features;

internal class CacheDataFileCache<T>(IDateTimeProvider dateTimeProvider) : FileCache<T>(dateTimeProvider)
{
    protected override string RootPath => FileSystem.Current.CacheDirectory;
}