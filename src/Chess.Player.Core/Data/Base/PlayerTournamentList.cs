using Chess.Player.Cache.Interfaces;

namespace Chess.Player.Data;

internal class PlayerTournamentList : List<PlayerTournament>, ICacheItem
{
    public DateTime? LastUpdateTime { get; set; }
}