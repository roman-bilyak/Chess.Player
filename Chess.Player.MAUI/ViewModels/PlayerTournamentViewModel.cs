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
        private int? _numberOfPlayers;

        [ObservableProperty]
        private int? _numberOfRounds;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private int? _rank;

        [ObservableProperty]
        private double? _points;
    }
}