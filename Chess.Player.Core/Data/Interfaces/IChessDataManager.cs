namespace Chess.Player.Data;

public delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}