using Chess.Player.Data;
using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerFullViewModel : BaseViewModel, IDisposable
{
    private readonly IChessDataService _chessDataService;
    private readonly IPlayerGroupService _playerGroupService;
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INavigationService _navigationService;
    private readonly IPopupService _popupService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNames))]
    private ObservableCollection<NameViewModel> _names = new();

    public bool HasNames => Names.Any();

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

    public int Years => _dateTimeProvider.UtcNow.Year - YearOfBirth ?? 0;

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
    private PlayerTournamentViewModel _selectedTournament;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    [ObservableProperty]
    private bool _forceRefresh;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ToggleFavoriteButtonName))]
    private bool _isFavorite;

    public string ToggleFavoriteButtonName => !IsFavorite ? "Add To Favorites" : "Remove From Favorites";

    public PlayerFullViewModel
    (
        IChessDataService chessDataService,
        IPlayerGroupService playerGroupService,
        IPlayerHistoryService historyService,
        IPlayerFavoriteService playerFavoriteService,
        IDateTimeProvider dateTimeProvider,
        INavigationService navigationService,
        IPopupService popupService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(playerGroupService);
        ArgumentNullException.ThrowIfNull(historyService);
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(navigationService);
        ArgumentNullException.ThrowIfNull(popupService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _playerGroupService = playerGroupService;
        _playerHistoryService = historyService;
        _playerFavoriteService = playerFavoriteService;
        _dateTimeProvider = dateTimeProvider;
        _navigationService = navigationService;
        _popupService = popupService;
        _serviceProvider = serviceProvider;

        _chessDataService.ProgressChanged += OnProgressChanged;
    }

    [RelayCommand]
    private Task StartAsync(CancellationToken cancellationToken)
    {
        ForceRefresh = false;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        ForceRefresh = true;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task AddNameAsync(CancellationToken cancellationToken)
    {
        string name = await _popupService.DisplayPromptAsync("Add Name", placeholder: "Example: Smith John");
        await _playerGroupService.AddToGroupAsync(Name, name, cancellationToken);

        IsLoading = true;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            PlayerFullInfo playerFullInfo = await _chessDataService.GetPlayerFullInfoAsync(Name, ForceRefresh, cancellationToken);

            int index = playerFullInfo.Tournaments.Count;
            _allTournaments = playerFullInfo.Tournaments.GroupBy(x => x.Tournament.EndDate?.Year)
                .ToDictionary(x => new TournamentYearViewModel
                {
                    Year = x.Key,
                    Years = x.Key - playerFullInfo.YearOfBirth ?? 0,
                    Count = x.Count()
                },
                x => x.Select(y =>
                {
                    PlayerTournamentViewModel viewModel = _serviceProvider.GetRequiredService<PlayerTournamentViewModel>();

                    viewModel.TournamentIndex = index--;
                    viewModel.TournamentId = y.Tournament.Id;
                    viewModel.TournamentName = y.Tournament.Name;
                    viewModel.TournamentLocation = y.Tournament.Location;
                    viewModel.TournamentStartDate = y.Tournament.StartDate;
                    viewModel.TournamentEndDate = y.Tournament.EndDate;
                    viewModel.NumberOfPlayers = y.Tournament.NumberOfPlayers;
                    viewModel.NumberOfRounds = y.Tournament.NumberOfRounds;
                    viewModel.Name = y.Player.Name;
                    viewModel.StartingRank = y.Player.StartingRank;
                    viewModel.Title = y.Player.Title;
                    viewModel.Rank = y.Player.Rank;
                    viewModel.Points = y.Player.Points;

                    return viewModel;
                }).ToList());

            Name = playerFullInfo.Name ?? Name;
            Names = new ObservableCollection<NameViewModel>(playerFullInfo.Names.Select(x => new NameViewModel { LastName = x.LastName, FirstName = x.FirstName}));
            Title = playerFullInfo.Title;
            FideId = playerFullInfo.FideId;
            ClubCity = playerFullInfo.ClubCity;
            YearOfBirth = playerFullInfo.YearOfBirth;
            IsFavorite = await _playerFavoriteService.ContainsAsync(Name, cancellationToken);

            TournamentYears = _allTournaments.Keys.ToList();
            TournamentYear = TournamentYears.FirstOrDefault(x => TournamentYear is null || x.Year == TournamentYear.Year);

            if (playerFullInfo.Tournaments.Any())
            {
                await _playerHistoryService.AddAsync(Name, cancellationToken);
            }

            Error = null;
        }
        catch (OperationCanceledException)
        {

        }
        catch (WebException)
        {
            Error = "No internet connection.";
        }
        catch
        {
            Error = "Oops! Something went wrong.";
        }
        finally
        {
            ForceRefresh = true;
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ItemSelectedAsync()
    {
        if (SelectedTournament == null 
            || !SelectedTournament.TournamentId.HasValue
            || !SelectedTournament.StartingRank.HasValue)
        {
            return;
        }

        await _navigationService.PushAsync<PlayerTournamentFullPage, PlayerTournamentFullViewModel>(x =>
        {
            x.TournamentId = SelectedTournament.TournamentId.Value;
            x.TournamentName = SelectedTournament.TournamentName;

            x.PlayerStartingRank = SelectedTournament.StartingRank.Value;
            x.PlayerName = SelectedTournament.Name;
        });

        SelectedTournament = null;
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(CancellationToken cancellationToken)
    {
        IsFavorite = await _playerFavoriteService.ToggleAsync(Name, cancellationToken);
    }

    [RelayCommand]
    private async Task OpenFideProfileAsync(string fideId, CancellationToken cancellationToken)
    {
        await Launcher.OpenAsync(new Uri($"https://ratings.fide.com/profile/{fideId}"));
    }

    private void OnProgressChanged(object sender, SearchProgressEventArgs e)
    {
        Progress = (double)e.ProgressPercentage / 100;
    }

    public void Dispose()
    {
        _chessDataService.ProgressChanged -= OnProgressChanged;
    }
}