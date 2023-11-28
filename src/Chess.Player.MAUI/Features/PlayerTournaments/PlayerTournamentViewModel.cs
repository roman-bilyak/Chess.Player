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

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? _tournamentName;

    [ObservableProperty]
    private int _playerNo;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string? _playerName;

    public string Title => $"{PlayerName} - {TournamentName}";

    [ObservableProperty]
    private PlayerTournamentShortViewModel _playerTournament;

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
        _playerTournament = serviceProvider.GetRequiredService<PlayerTournamentShortViewModel>();
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        DateTime currentDate = _dateTimeProvider.UtcNow.Date;

        PlayerTournamentInfo playerTournamentInfo = await _chessDataService.GetPlayerTournamentInfoAsync(TournamentId, PlayerNo, UseCache, cancellationToken);

        PlayerTournament.TournamentId = playerTournamentInfo.Tournament.Id;
        PlayerTournament.TournamentName = playerTournamentInfo.Tournament.Name;
        PlayerTournament.TournamentLocation = playerTournamentInfo.Tournament.Location;
        PlayerTournament.TournamentStartDate = playerTournamentInfo.Tournament.StartDate;
        PlayerTournament.TournamentEndDate = playerTournamentInfo.Tournament.EndDate;
        PlayerTournament.IsOnline = playerTournamentInfo.Tournament.IsOnline(currentDate);
        PlayerTournament.IsFuture = playerTournamentInfo.Tournament.IsFuture(currentDate);
        PlayerTournament.NumberOfPlayers = playerTournamentInfo.Tournament.Players.Count;
        PlayerTournament.NumberOfRounds = playerTournamentInfo.Tournament.NumberOfRounds;
        PlayerTournament.Name = playerTournamentInfo.Player.Name;
        PlayerTournament.No = playerTournamentInfo.Player.No;
        PlayerTournament.Title = playerTournamentInfo.Player.Title;
        PlayerTournament.Rank = playerTournamentInfo.Player.Rank;
        PlayerTournament.Points = playerTournamentInfo.Player.Points;

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