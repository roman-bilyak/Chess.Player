using Chess.Player.Data;
using Chess.Player.MAUI.Features;
using Chess.Player.MAUI.Features.Favorites;
using Chess.Player.MAUI.Features.Home;
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
    private readonly IPopupService _popupService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNames))]
    private ObservableCollection<NameViewModel> _names = [];

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

    private Dictionary<TournamentYearViewModel, List<PlayerTournamentShortViewModel>> _allTournaments = [];

    [ObservableProperty]
    private ObservableCollection<TournamentYearViewModel> _tournamentYears = [];

    public bool HasTournamentYears => TournamentYears?.Any() ?? false;

    [ObservableProperty]
    private TournamentYearViewModel? _tournamentYear;

    [ObservableProperty]
    private ObservableCollection<PlayerTournamentShortViewModel> _tournaments = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ToggleFavoriteButtonName))]
    private bool _isFavorite;

    public string ToggleFavoriteButtonName => !IsFavorite ? "Add To Favorites" : "Remove From Favorites";

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public PlayerFullViewModel
    (
        IChessDataService chessDataService,
        IPlayerGroupService playerGroupService,
        IPlayerHistoryService historyService,
        IPlayerFavoriteService playerFavoriteService,
        IDateTimeProvider dateTimeProvider,
        IPopupService popupService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(playerGroupService);
        ArgumentNullException.ThrowIfNull(historyService);
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(popupService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _playerGroupService = playerGroupService;
        _playerHistoryService = historyService;
        _playerFavoriteService = playerFavoriteService;
        _dateTimeProvider = dateTimeProvider;
        _popupService = popupService;
        _serviceProvider = serviceProvider;

        _chessDataService.ProgressChanged += OnProgressChanged;
    }

    [RelayCommand]
    private Task StartAsync(CancellationToken cancellationToken)
    {
        UseCache = true;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        UseCache = false;
        IsLoading = true;

        return Task.CompletedTask;
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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(Name))
            {
                return;
            }

            DateTime currentDate = _dateTimeProvider.UtcNow.Date;

            PlayerFullInfo playerFullInfo = await _chessDataService.GetPlayerFullInfoAsync(Name, UseCache, cancellationToken);
            bool isFavorite = playerFullInfo.Name is not null 
                && await _playerFavoriteService.ContainsAsync(playerFullInfo.Name, cancellationToken);
            if (playerFullInfo.Tournaments.Count > 0 && !string.IsNullOrEmpty(playerFullInfo.Name))
            {
                await _playerHistoryService.AddAsync(playerFullInfo.Name, cancellationToken);
            }

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
            Names = new ObservableCollection<NameViewModel>(playerFullInfo.Names.Select(x => new NameViewModel { LastName = x.LastName, FirstName = x.FirstName}));
            Title = playerFullInfo.Title;
            FideId = playerFullInfo.FideId;
            ClubCity = playerFullInfo.ClubCity;
            YearOfBirth = playerFullInfo.YearOfBirth;
            IsFavorite = isFavorite;

            TournamentYears.Clear();
            foreach(TournamentYearViewModel tournamentYear in _allTournaments.Keys.ToList())
            {
                TournamentYears.Add(tournamentYear);
            }
            OnPropertyChanged(nameof(HasTournamentYears));

            TournamentYear = TournamentYears.FirstOrDefault(x => TournamentYear is null || x.Year == TournamentYear.Year);

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
            UseCache = false;
            IsLoading = false;
        }
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

        IsFavorite = await _playerFavoriteService.ToggleAsync(Name, cancellationToken);
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