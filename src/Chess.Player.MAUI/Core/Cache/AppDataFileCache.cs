using Chess.Player.Cache;
using Chess.Player.Data;

namespace Chess.Player.MAUI.Cache;

internal class AppDataFileCache<T>(IDateTimeProvider dateTimeProvider) : FileCache<T>(dateTimeProvider)
{
    protected override string RootPath => FileSystem.Current.AppDataDirectory;
}