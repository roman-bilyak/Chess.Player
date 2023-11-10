using Chess.Player.Cache.Interfaces;

namespace Chess.Player.Data;

public class PlayerTournamentInfo : ICacheItem
{
    public TournamentInfo Tournament { get; private set; }

    public PlayerInfo Player { get; private set; }

    public DateTime? LastUpdateTime { get; set; }

    public PlayerTournamentInfo(TournamentInfo tournament, PlayerInfo player)
    {
        ArgumentNullException.ThrowIfNull(tournament);
        ArgumentNullException.ThrowIfNull(player);

        Tournament = tournament;
        Player = player;
    }
}