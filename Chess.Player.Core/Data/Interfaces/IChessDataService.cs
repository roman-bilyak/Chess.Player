namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool forceRefresh, CancellationToken cancellationToken);
}