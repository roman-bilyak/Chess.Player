using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        private ObservableCollection<PlayerTournamentYearViewModel> _tournaments = new();

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

                Tournaments.Clear();

                int index = searchResult.Count;
                foreach (var yearGroup in searchResult.GroupBy(x => x.Tournament.EndDate?.Year))
                {
                    PlayerTournamentYearViewModel yearGroupViewModel = new()
                    {
                        Year = yearGroup.Key,
                        YearOfBirth = searchResult.YearOfBirth
                    };

                    foreach (var item in yearGroup)
                    {
                        yearGroupViewModel.Add(new PlayerTournamentViewModel
                        {
                            No = index--,
                            TournamentName = item.Tournament.Name,
                            TournamentLocation = item.Tournament.Location,
                            TournamentStartDate = item.Tournament.StartDate,
                            TournamentEndDate = item.Tournament.EndDate,
                            ClubCity = item.Player.ClubCity,
                            Title = item.Player.Title,
                            Points = item.Player.Points,
                            NumberOfRounds = item.Tournament.NumberOfRounds,
                            Rank = item.Player.Rank,
                            NumberOfPlayers = item.Tournament.NumberOfPlayers,
                        });
                    }
                    Tournaments.Add(yearGroupViewModel);
                }
                

                //foreach (PlayerTournamentInfo playerTournamentInfo in searchResult)
                //{
                //    Tournaments.Add(new PlayerTournamentViewModel
                //    {
                //        No = index--,
                //        TournamentName = playerTournamentInfo.Tournament.Name,
                //        TournamentStartDate = playerTournamentInfo.Tournament.StartDate,
                //        TournamentEndDate = playerTournamentInfo.Tournament.EndDate,
                //        ClubCity = playerTournamentInfo.Player.ClubCity,
                //        Title = playerTournamentInfo.Player.Title,
                //        Points = playerTournamentInfo.Player.Points,
                //        NumberOfRounds = playerTournamentInfo.Tournament.NumberOfRounds,
                //        Rank = playerTournamentInfo.Player.Rank,
                //        NumberOfPlayers = playerTournamentInfo.Tournament.NumberOfPlayers,
                //    });
                //}
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