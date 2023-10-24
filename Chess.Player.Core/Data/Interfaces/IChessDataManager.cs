namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}