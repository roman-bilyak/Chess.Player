namespace Chess.Player.Data
{
    internal class ChessDataManager : IChessDataManager
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

        public async Task SearchAsync(SearchCriteria[] searchCriterias)
        {
            SearchResult searchResult = await _dataService.SearchAsync(searchCriterias);
            _outputFormatter.DisplayResult(searchResult);
        }
    }
}