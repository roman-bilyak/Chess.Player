using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class PlayerTournamentViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoAndTournamentName))]
        private int _tournamentNo;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoAndTournamentName))]
        private string _tournamentName;

        public string NoAndTournamentName => $"{TournamentNo}. {TournamentName}";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
        private DateTime? _tournamentStartDate;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
        private DateTime? _tournamentEndDate;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
        private string _tournamentLocation;

        public string TournamentDateAndLocation => TournamentStartDate == TournamentEndDate
            ? $"{TournamentEndDate:dd.MM} - {TournamentLocation}"
            : $"{TournamentStartDate:dd.MM} - {TournamentEndDate:dd.MM} - {TournamentLocation}";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasClubCity))]
        private string _clubCity;

        public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PointsAndRounds))]
        private double? _points;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PointsAndRounds))]
        private int? _numberOfRounds;

        public string PointsAndRounds => $"{Points}/{NumberOfRounds}";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RankAndPlayers))]
        private int? _rank;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RankAndPlayers))]
        private int? _numberOfPlayers;

        public string RankAndPlayers => $"{Rank}/{NumberOfPlayers}";
    }
}