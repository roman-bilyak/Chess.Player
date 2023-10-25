namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetFullPlayerInfoAsync(string name, bool forceRefresh, CancellationToken cancellationToken);
}