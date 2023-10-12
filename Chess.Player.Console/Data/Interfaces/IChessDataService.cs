namespace Chess.Player.Data
{
    internal delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

    internal interface IChessDataService
    {
        event SearchProgressEventHandler? ProgressChanged;

        Task<SearchResult> SearchAsync(params SearchCriteria[] searchCriterias);
    }
}