namespace Chess.Player.Data;

public interface IPlayerGroupService
{
    Task AddToGroupAsync(string groupName, string name, CancellationToken cancellationToken);

    Task<PlayerGroupInfo> GetGroupInfoAsync(string name, CancellationToken cancellationToken);
}