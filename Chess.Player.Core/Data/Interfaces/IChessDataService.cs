namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetFullPlayerInfoAsync(SearchCriteria[] searchCriterias, bool forceRefresh, CancellationToken cancellationToken);
}