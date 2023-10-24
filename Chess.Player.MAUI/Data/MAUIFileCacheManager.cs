namespace Chess.Player.Data
{
    internal class MAUIFileCacheManager : FileCacheManager
    {
        protected override string RootPath => Path.Combine(FileSystem.Current.AppDataDirectory, "Cache");
    }
}