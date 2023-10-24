namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}