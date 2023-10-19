using Chess.Player.Data;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class PlayerViewModel : ObservableObject
    {
        private readonly IChessDataService _chessDataService;
        private readonly IPopupService _popupService;

        public string Name => Names?.FirstOrDefault();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        private ObservableCollection<string> _names = new();

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
        [NotifyPropertyChangedFor(nameof(HasTournamentYears))]
        private List<TournamentYearViewModel> _tournamentYears;

        public bool HasTournamentYears => TournamentYears?.Any() ?? false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Tournaments))]
        private TournamentYearViewModel _tournamentYear;

        private Dictionary<TournamentYearViewModel, List<PlayerTournamentViewModel>> _allTournaments;

        public List<PlayerTournamentViewModel> Tournaments => _allTournaments?.GetValueOrDefault(TournamentYear) ?? new List<PlayerTournamentViewModel>();

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private double _progress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string _error;

        public bool HasError => !string.IsNullOrWhiteSpace(Error);

        public PlayerViewModel
        (
            IChessDataService chessDataService,
            IPopupService popupService
        )
        {
            ArgumentNullException.ThrowIfNull(chessDataService);
            ArgumentNullException.ThrowIfNull(popupService);

            _chessDataService = chessDataService;
            _popupService = popupService;

            _chessDataService.ProgressChanged += (sender, e) =>
            {
                Progress = (double)e.ProgressPercentage / 100;
            };
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
                SearchCriteria[] searchCriterias = Names.Select(x => new SearchCriteria(x)).ToArray();
                SearchResult searchResult = await _chessDataService.SearchAsync(searchCriterias, cancellationToken);

                Names.Clear();
                foreach(var name in searchResult.Names)
                {
                    Names.Add(name);
                }

                Title = searchResult.Title;
                FideId = searchResult.FideId;
                ClubCity = searchResult.ClubCity;
                YearOfBirth = searchResult.YearOfBirth;

                int index = searchResult.Count;
                _allTournaments = searchResult.GroupBy(x => x.Tournament.EndDate?.Year)
                    .ToDictionary(x => new TournamentYearViewModel
                    {
                        Year = x.Key,
                        Years = x.Key - YearOfBirth ?? 0,
                        Count = x.Count()
                    },
                    x => x.Select(y => new PlayerTournamentViewModel
                    {
                        TournamentNo = index--,
                        TournamentName = y.Tournament.Name,
                        TournamentLocation = y.Tournament.Location,
                        TournamentStartDate = y.Tournament.StartDate,
                        TournamentEndDate = y.Tournament.EndDate,
                        NumberOfPlayers = y.Tournament.NumberOfPlayers,
                        NumberOfRounds = y.Tournament.NumberOfRounds,
                        Title = y.Player.Title,
                        Rank = y.Player.Rank,
                        Points = y.Player.Points,
                    }).ToList());

                TournamentYears = _allTournaments.Keys.ToList();
                TournamentYear = TournamentYears.FirstOrDefault(x => TournamentYear is null || x.Year == TournamentYear.Year);

                Error = null;
            }
            catch
            {
                Error = "Something went wrong. Please try again later.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddNameAsync()
        {
            string name = await _popupService.DisplayPromptAsync("Add Name", placeholder: "Example: Smith John");
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return;
            }

            Names.Add(name);
            IsLoading = true;
        }
    }
}