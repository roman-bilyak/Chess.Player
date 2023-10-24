namespace Chess.Player.Data;

internal class ChessDataService : IChessDataService
{
    private readonly IChessDataManager _chessDataManager;
    private readonly IChessDataNormalizer _chessDataNormalizer;
    private readonly ICacheManager _cacheManager;

    public event SearchProgressEventHandler? ProgressChanged
    {
        add
        {
            _chessDataManager.ProgressChanged += value;
        }
        remove
        {
            _chessDataManager.ProgressChanged -= value;
        }
    }

    public ChessDataService
    (
        IChessDataManager chessDataManager,
        IChessDataNormalizer chessDataNormalizer,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataManager);
        ArgumentNullException.ThrowIfNull(chessDataNormalizer);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataManager = chessDataManager;
        _chessDataNormalizer = chessDataNormalizer;
        _cacheManager = cacheManager;
    }

    public async Task<SearchResult> SearchAsync(SearchCriteria[] searchCriterias, bool forceRefresh, CancellationToken cancellationToken)
    {
        foreach (SearchCriteria searchCriteria in searchCriterias)
        {
            searchCriteria.Name = _chessDataNormalizer.NormalizeName(searchCriteria.Name);
        }

        return await _cacheManager.GetOrAddAsync(nameof(SearchResult),
                string.Join("_", searchCriterias.Select(x => x.Name)),
                async () => await _chessDataManager.SearchAsync(searchCriterias, cancellationToken),
                forceRefresh,
                cancellationToken);
    }
}