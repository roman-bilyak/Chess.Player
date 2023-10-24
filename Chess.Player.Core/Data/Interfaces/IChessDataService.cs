namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, bool forceRefresh, CancellationToken cancellationToken);
}