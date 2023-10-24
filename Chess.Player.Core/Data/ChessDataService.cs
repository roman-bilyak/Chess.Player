namespace Chess.Player.Data;

public class ChessDataService : IChessDataService
{
    private readonly IChessDataManager _chessDataManager;
    private readonly IOutputFormatter _outputFormatter;

    public ChessDataService
    (
        IChessDataManager chessDataManager,
        IOutputFormatter outputFormatter)
    {
        ArgumentNullException.ThrowIfNull(chessDataManager);
        ArgumentNullException.ThrowIfNull(outputFormatter);

        _chessDataManager = chessDataManager;
        _outputFormatter = outputFormatter;

        _chessDataManager.ProgressChanged += (sender, e) =>
        {
            _outputFormatter.DisplayProgress(e.ProgressPercentage);
        };
    }

    public async Task SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken)
    {
        SearchResult searchResult = await _chessDataManager.SearchAsync(searchCriterias, cancellationToken);
        _outputFormatter.DisplayResult(searchResult);
    }
}