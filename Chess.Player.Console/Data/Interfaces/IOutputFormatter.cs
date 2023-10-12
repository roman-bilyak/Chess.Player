namespace Chess.Player.Data
{
    internal interface IOutputFormatter
    {
        void DisplayProgress(int progressPercentage);

        void DisplayResult(SearchResult searchResult);
    }
}