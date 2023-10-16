using System.Threading;

namespace Chess.Player.Data
{
    public class ChessDataManager : IChessDataManager
    {
        private readonly IChessDataService _dataService;
        private readonly IOutputFormatter _outputFormatter;

        public ChessDataManager(IChessDataService dataService, IOutputFormatter outputFormatter)
        {
            ArgumentNullException.ThrowIfNull(dataService);
            ArgumentNullException.ThrowIfNull(outputFormatter);

            _dataService = dataService;
            _outputFormatter = outputFormatter;

            _dataService.ProgressChanged += (sender, e) =>
            {
                _outputFormatter.DisplayProgress(e.ProgressPercentage);
            };
        }

        public async Task SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken)
        {
            SearchResult searchResult = await _dataService.SearchAsync(searchCriterias, cancellationToken);
            _outputFormatter.DisplayResult(searchResult);
        }
    }
}