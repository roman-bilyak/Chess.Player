namespace Chess.Player.Data;

public interface IChessDataService
{
    Task SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);
}