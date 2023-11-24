using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentViewModel : BaseViewModel
{
    private readonly IChessDataService _chessDataService;
    private readonly IDateTimeProvider _dateTimeProvider;
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

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public PlayerTournamentViewModel
    (
        IChessDataService chessDataService,
        IDateTimeProvider dateTimeProvider,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _dateTimeProvider = dateTimeProvider;
        _serviceProvider = serviceProvider;

        _playerTournament = serviceProvider.GetRequiredService<PlayerTournamentShortViewModel>();
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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
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
            PlayerTournament.ShowFullInfo = true;

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
}