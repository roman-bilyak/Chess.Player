namespace Chess.Player.Data
{
    public interface IOutputFormatter
    {
        void DisplayProgress(int progressPercentage);

        void DisplayResult(SearchResult searchResult);
    }
}