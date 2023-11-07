using Chess.Player.Cache;

namespace Chess.Player.MAUI.Cache;

internal class AppDataFileCache<T> : FileCache<T>
{
    protected override string RootPath => FileSystem.Current.AppDataDirectory;
}