namespace Chess.Player.Data
{
    internal class ConsoleFileCacheManager : FileCacheManager
    {
        protected override string RootPath => "Cache";
    }
}