namespace Chess.Player.Data
{
    internal class MAUIFileCacheManager: FileCacheManager
    {
        protected override string RootPath => FileSystem.Current.AppDataDirectory;
    }
}