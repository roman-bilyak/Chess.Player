using Chess.Player.Cache;
using Chess.Player.Data;

namespace Chess.Player.MAUI.Features;

internal class AppDataFileCache<T> : FileCache<T>
{
    public AppDataFileCache(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
    }

    protected override string RootPath => FileSystem.Current.AppDataDirectory;
}