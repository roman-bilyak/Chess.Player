namespace Chess.Player.Data
{
    public interface IChessDataManager
    {
        Task SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
    }
}