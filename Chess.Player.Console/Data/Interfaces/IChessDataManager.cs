namespace Chess.Player.Data
{
    internal interface IChessDataManager
    {
        Task SearchAsync(SearchCriteria[] searchCriterias);
    }
}