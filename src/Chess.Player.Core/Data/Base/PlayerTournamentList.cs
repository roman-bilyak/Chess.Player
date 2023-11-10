using Chess.Player.Cache.Interfaces;

namespace Chess.Player.Data;

public class PlayerTournamentList : ICacheItem
{
    public List<PlayerTournament> Items { get; protected set; } = new List<PlayerTournament>();

    public DateTime? LastUpdateTime { get; set; }
}