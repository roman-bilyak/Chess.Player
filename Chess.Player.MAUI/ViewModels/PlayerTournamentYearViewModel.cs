using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels
{
    internal class PlayerTournamentYearViewModel: ObservableCollection<PlayerTournamentViewModel>
    {
        public int? Year { get; set; }

        public int? YearOfBirth { get; set; }

        public string YearGroup => $"{Year} ({Year - YearOfBirth} years)";
    }
}