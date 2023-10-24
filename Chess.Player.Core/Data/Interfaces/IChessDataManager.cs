namespace Chess.Player.Data;

public delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

public interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}