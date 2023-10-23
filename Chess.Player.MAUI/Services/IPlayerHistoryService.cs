namespace Chess.Player.MAUI.Services;

public interface IPlayerHistoryService
{
    Task AddAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetAllAsync(CancellationToken cancellationToken);
}
