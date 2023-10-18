using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class PlayerViewModel : ObservableValidator
    {
        private readonly IChessDataService _chessDataService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _lastName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _firstName;

        public string FullName => $"{LastName} {FirstName}";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasTitle))]
        private string _title;

        public bool HasTitle => !string.IsNullOrEmpty(Title);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasFideId))]
        private string _fideId;

        public bool HasFideId => !string.IsNullOrEmpty(FideId);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasClubCity))]
        private string _clubCity;

        public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Years), nameof(HasYearOfBirth))]
        private int? _yearOfBirth;

        public bool HasYearOfBirth => YearOfBirth is not null;

        public int Years => DateTime.UtcNow.Year - YearOfBirth ?? 0;


        [ObservableProperty]
        private List<int?> _tournamentYears;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Tournaments))]
        private int? _tournamentYear;

        private Dictionary<int?, List<PlayerTournamentViewModel>> _allTournaments;

        public List<PlayerTournamentViewModel> Tournaments => _allTournaments?.GetValueOrDefault(TournamentYear) ?? new List<PlayerTournamentViewModel>();

        [ObservableProperty]
        private bool _isLoading = false;

        public PlayerViewModel(IChessDataService chessDataService)
        {
            ArgumentNullException.ThrowIfNull(chessDataService);

            _chessDataService = chessDataService;
        }

        [RelayCommand]
        private void Start()
        {
            IsLoading = true;
        }

        [RelayCommand(IncludeCancelCommand = true)]
        private async Task LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                SearchResult searchResult = await _chessDataService.SearchAsync(new[] { new SearchCriteria(LastName, FirstName) }, cancellationToken);

                Title = searchResult.Title;
                FideId = searchResult.FideId;
                ClubCity = searchResult.ClubCity;
                YearOfBirth = searchResult.YearOfBirth;

                int index = searchResult.Count;
                _allTournaments = searchResult.GroupBy(x => x.Tournament.EndDate?.Year)
                    .ToDictionary(x => x.Key, x => x.Select(y => new PlayerTournamentViewModel
                    {
                        TournamentNo = index--,
                        TournamentName = y.Tournament.Name,
                        TournamentLocation = y.Tournament.Location,
                        TournamentStartDate = y.Tournament.StartDate,
                        TournamentEndDate = y.Tournament.EndDate,
                        ClubCity = y.Player.ClubCity,
                        Title = y.Player.Title,
                        Points = y.Player.Points,
                        NumberOfRounds = y.Tournament.NumberOfRounds,
                        Rank = y.Player.Rank,
                        NumberOfPlayers = y.Tournament.NumberOfPlayers,
                    }).ToList());

                TournamentYears = _allTournaments.Keys.ToList();
                TournamentYear ??= TournamentYears.FirstOrDefault();
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}