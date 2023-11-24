using Chess.Player.Data;
using Chess.Player.MAUI.Features.Favorites;
using Chess.Player.MAUI.Features.Home;
using Chess.Player.MAUI.Features.PlayerTournaments;
using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Players;

public partial class PlayerViewModel : BaseRefreshViewModel, IDisposable
{
    private readonly IChessDataService _chessDataService;
    private readonly IPlayerGroupService _playerGroupService;
    private readonly IHistoryService _historyService;
    private readonly IFavoriteService _favoriteService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPopupService _popupService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNames))]
    private ObservableCollection<PlayerNameViewModel> _names = [];

    public bool HasNames => Names.Any();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTitle))]
    private string? _title;

    public bool HasTitle => !string.IsNullOrEmpty(Title);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasFideId))]
    private string? _fideId;

    public bool HasFideId => !string.IsNullOrEmpty(FideId);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasClubCity))]
    private string? _clubCity;

    public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Years), nameof(HasYearOfBirth))]
    private int? _yearOfBirth;

    public bool HasYearOfBirth => YearOfBirth is not null;

    public int Years => _dateTimeProvider.UtcNow.Year - YearOfBirth ?? 0;

    private Dictionary<PlayerTournamentYearViewModel, List<PlayerTournamentShortViewModel>> _allTournaments = [];

    [ObservableProperty]
    private ObservableCollection<PlayerTournamentYearViewModel> _tournamentYears = [];

    public bool HasTournamentYears => TournamentYears?.Any() ?? false;

    [ObservableProperty]
    private PlayerTournamentYearViewModel? _tournamentYear;

    [ObservableProperty]
    private ObservableCollection<PlayerTournamentShortViewModel> _tournaments = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ToggleFavoriteButtonName))]
    private bool _isFavorite;

    public string ToggleFavoriteButtonName => !IsFavorite ? "Add To Favorites" : "Remove From Favorites";

    public PlayerViewModel
    (
        IChessDataService chessDataService,
        IPlayerGroupService playerGroupService,
        IHistoryService historyService,
        IFavoriteService favoriteService,
        IDateTimeProvider dateTimeProvider,
        IPopupService popupService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(playerGroupService);
        ArgumentNullException.ThrowIfNull(historyService);
        ArgumentNullException.ThrowIfNull(favoriteService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(popupService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _playerGroupService = playerGroupService;
        _historyService = historyService;
        _favoriteService = favoriteService;
        _dateTimeProvider = dateTimeProvider;
        _popupService = popupService;
        _serviceProvider = serviceProvider;

        _chessDataService.ProgressChanged += OnProgressChanged;
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        DateTime currentDate = _dateTimeProvider.UtcNow.Date;

        PlayerFullInfo playerFullInfo = await _chessDataService.GetPlayerFullInfoAsync(Name, UseCache, cancellationToken);
        bool isFavorite = playerFullInfo.Name is not null
            && await _favoriteService.ContainsAsync(playerFullInfo.Name, cancellationToken);
        if (playerFullInfo.Tournaments.Count > 0 && !string.IsNullOrEmpty(playerFullInfo.Name))
        {
            await _historyService.AddAsync(playerFullInfo.Name, cancellationToken);
        }

        int index = playerFullInfo.Tournaments.Count;
        _allTournaments = playerFullInfo.Tournaments.GroupBy(x => x.Tournament.EndDate?.Year)
            .ToDictionary(x => new PlayerTournamentYearViewModel
            {
                Year = x.Key,
                Years = x.Key - playerFullInfo.YearOfBirth ?? 0,
                Count = x.Count()
            },
            x => x.Select(y =>
            {
                PlayerTournamentShortViewModel viewModel = _serviceProvider.GetRequiredService<PlayerTournamentShortViewModel>();

                viewModel.TournamentIndex = index--;
                viewModel.TournamentId = y.Tournament.Id;
                viewModel.TournamentName = y.Tournament.Name;
                viewModel.TournamentLocation = y.Tournament.Location;
                viewModel.TournamentStartDate = y.Tournament.StartDate;
                viewModel.TournamentEndDate = y.Tournament.EndDate;
                viewModel.IsOnline = y.Tournament.IsOnline(currentDate);
                viewModel.IsFuture = y.Tournament.IsFuture(currentDate);
                viewModel.NumberOfPlayers = y.Tournament.Players.Count;
                viewModel.NumberOfRounds = y.Tournament.NumberOfRounds;
                viewModel.Name = y.Player.Name;
                viewModel.No = y.Player.No;
                viewModel.Title = y.Player.Title;
                viewModel.Rank = y.Player.Rank;
                viewModel.Points = y.Player.Points;

                return viewModel;
            }).ToList());

        Name = playerFullInfo.Name ?? Name;
        Names = new ObservableCollection<PlayerNameViewModel>(playerFullInfo.Names.Select(x => new PlayerNameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
        Title = playerFullInfo.Title;
        FideId = playerFullInfo.FideId;
        ClubCity = playerFullInfo.ClubCity;
        YearOfBirth = playerFullInfo.YearOfBirth;
        IsFavorite = isFavorite;

        TournamentYears.Clear();
        foreach (PlayerTournamentYearViewModel tournamentYear in _allTournaments.Keys.ToList())
        {
            TournamentYears.Add(tournamentYear);
        }
        OnPropertyChanged(nameof(HasTournamentYears));

        TournamentYear = TournamentYears.FirstOrDefault(x => TournamentYear is null || x.Year == TournamentYear.Year);
    }

    [RelayCommand]
    private void ChangeTournamentYear()
    {
        Tournaments.Clear();

        if (TournamentYear is null)
        {
            return;
        }

        foreach (PlayerTournamentShortViewModel viewModel in _allTournaments.GetValueOrDefault(TournamentYear) ?? [])
        {
            Tournaments.Add(viewModel);
        }
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        IsFavorite = await _favoriteService.ToggleAsync(Name, cancellationToken);
    }

    [RelayCommand]
    private async Task AddNameAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        string name = await _popupService.DisplayPromptAsync("Add Name", placeholder: "Example: Smith John");
        await _playerGroupService.AddToGroupAsync(Name, name, cancellationToken);

        IsLoading = true;
    }

    [RelayCommand]
    private static async Task OpenFideProfileAsync(string fideId, CancellationToken cancellationToken)
    {
        await Launcher.OpenAsync(new Uri($"https://ratings.fide.com/profile/{fideId}"));
    }

    private void OnProgressChanged(object sender, SearchProgressEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Progress = (double)e.ProgressPercentage / 100;
        });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _chessDataService.ProgressChanged -= OnProgressChanged;
        }
    }
}