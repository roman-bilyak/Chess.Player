namespace Chess.Player.MAUI.Services;

public interface IPlayerFavoriteService
{
    Task AddAsync(string name, CancellationToken cancellationToken);

    Task RemoveAsync(string name, CancellationToken cancellationToken);

    Task<bool> ToggleAsync(string name, CancellationToken cancellationToken);

    Task<bool> ContainsAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetAllAsync(CancellationToken cancellationToken);
}