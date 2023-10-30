namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}