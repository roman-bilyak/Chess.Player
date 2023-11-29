using Chess.Player.Data;
using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentViewModel : BaseRefreshViewModel
{
    private readonly IChessDataService _chessDataService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    public string Title => $"{PlayerName} - {TournamentName}";

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private DateTime? _tournamentStartDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private DateTime? _tournamentEndDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private string? _tournamentLocation;

    public string TournamentDateAndLocation => TournamentStartDate == TournamentEndDate
        ? $"{TournamentEndDate:dd.MM} - {TournamentLocation}"
        : $"{TournamentStartDate:dd.MM} - {TournamentEndDate:dd.MM} - {TournamentLocation}";

    [ObservableProperty]
    private int? _tournamentNumberOfPlayers;

    [ObservableProperty]
    private int? _tournamentNumberOfRounds;

    [ObservableProperty]
    private bool _tournamentIsOnline;

    [ObservableProperty]
    private bool _tournamentIsFuture;

    [ObservableProperty]
    private int _playerNo;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? _playerName;

    [ObservableProperty]
    private string? _playerTitle;

    [ObservableProperty]
    private int? _playerRank;

    [ObservableProperty]
    private double? _playerPoints;

    public bool HasGames => Games?.Any() ?? false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasGames))]
    private ObservableCollection<PlayerTournamentGameViewModel> _games = [];

    public PlayerTournamentViewModel
    (
        IChessDataService chessDataService,
        IDateTimeProvider dateTimeProvider,
        INavigationService navigationService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(navigationService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _dateTimeProvider = dateTimeProvider;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        DateTime currentDate = _dateTimeProvider.UtcNow.Date;

        PlayerTournamentInfo playerTournamentInfo = await _chessDataService.GetPlayerTournamentInfoAsync(TournamentId, PlayerNo, UseCache, cancellationToken);

        TournamentName = playerTournamentInfo.Tournament.Name;
        TournamentStartDate = playerTournamentInfo.Tournament.StartDate;
        TournamentEndDate = playerTournamentInfo.Tournament.EndDate;
        TournamentLocation = playerTournamentInfo.Tournament.Location;
        TournamentNumberOfPlayers = playerTournamentInfo.Tournament.Players.Count;
        TournamentNumberOfRounds = playerTournamentInfo.Tournament.NumberOfRounds;
        TournamentIsOnline = playerTournamentInfo.Tournament.IsOnline(currentDate);
        TournamentIsFuture = playerTournamentInfo.Tournament.IsFuture(currentDate);

        PlayerName = playerTournamentInfo.Player.Name;
        PlayerTitle = playerTournamentInfo.Player.Title;
        PlayerRank = playerTournamentInfo.Player.Rank;
        PlayerPoints = playerTournamentInfo.Player.Points;

        Games.Clear();
        foreach (GameInfo gameInfo in playerTournamentInfo.Player.Games.OrderByDescending(x => x.Round))
        {
            PlayerTournamentGameViewModel gameViewModel = _serviceProvider.GetRequiredService<PlayerTournamentGameViewModel>();

            gameViewModel.TournamentId = playerTournamentInfo.Tournament.Id;
            gameViewModel.TournamentName = playerTournamentInfo.Tournament.Name;
            gameViewModel.Round = gameInfo.Round;
            gameViewModel.Board = gameInfo.Board;
            gameViewModel.No = gameInfo.No;
            gameViewModel.Name = gameInfo.Name;
            gameViewModel.ClubCity = gameInfo.ClubCity;
            gameViewModel.IsWhiteBlack = gameInfo.IsWhite;
            gameViewModel.Result = gameInfo.Result;

            Games.Add(gameViewModel);
        }
        OnPropertyChanged(nameof(HasGames));
    }

    [RelayCommand]
    private async Task ShowPlayerInfoAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(PlayerName))
        {
            return;
        }

        await _navigationService.NavigateToPlayerAsync(PlayerName);
    }

    [RelayCommand]
    private async Task ShowTournamentInfoAsync(CancellationToken cancellationToken)
    {
        await _navigationService.NavigateToTournamentAsync(TournamentId, TournamentName);
    }
}