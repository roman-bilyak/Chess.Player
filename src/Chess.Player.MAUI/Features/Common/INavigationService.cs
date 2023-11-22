namespace Chess.Player.MAUI.Features;

public interface INavigationService
{
    Task NavigateToPlayerAsync(string playerName);

    Task NavigateToTournamentAsync(int tournamentId, string? tournamentName);

    Task NavigateToPlayerTournamentAsync(int tournamentId, string? tournamentName, int playerNo, string? playerName);
}