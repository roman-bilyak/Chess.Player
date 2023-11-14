using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentFullViewModel : BaseViewModel
{
    private readonly IChessDataService _chessDataService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    private int _playerStartingRank;

    [ObservableProperty]
    private string _playerName;

    [ObservableProperty]
    private PlayerTournamentViewModel _playerTournament;

    [ObservableProperty]
    private GameListViewModel _gameList;

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public PlayerTournamentFullViewModel
    (
        IChessDataService chessDataService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _serviceProvider = serviceProvider;

        _playerTournament = serviceProvider.GetRequiredService<PlayerTournamentViewModel>();
        _gameList = serviceProvider.GetRequiredService<GameListViewModel>();
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
            PlayerTournamentInfo playerTournamentInfo = await _chessDataService.GetPlayerTournamentInfoAsync(TournamentId, PlayerStartingRank, UseCache, cancellationToken);

            PlayerTournament.TournamentId = playerTournamentInfo.Tournament.Id;
            PlayerTournament.TournamentName = playerTournamentInfo.Tournament.Name;
            PlayerTournament.TournamentLocation = playerTournamentInfo.Tournament.Location;
            PlayerTournament.TournamentStartDate = playerTournamentInfo.Tournament.StartDate;
            PlayerTournament.TournamentEndDate = playerTournamentInfo.Tournament.EndDate;
            PlayerTournament.NumberOfPlayers = playerTournamentInfo.Tournament.Players.Count;
            PlayerTournament.NumberOfRounds = playerTournamentInfo.Tournament.NumberOfRounds;
            PlayerTournament.Name = playerTournamentInfo.Player.Name;
            PlayerTournament.StartingRank = playerTournamentInfo.Player.StartingRank;
            PlayerTournament.Title = playerTournamentInfo.Player.Title;
            PlayerTournament.Rank = playerTournamentInfo.Player.Rank;
            PlayerTournament.Points = playerTournamentInfo.Player.Points;
            PlayerTournament.ShowFullInfo = true;

            GameList.Games.Clear();
            foreach(GameInfo gameInfo in playerTournamentInfo.Player.Games.OrderByDescending(x => x.Round))
            {
                GameViewModel gameViewModel = _serviceProvider.GetRequiredService<GameViewModel>();

                gameViewModel.Round = gameInfo.Round;
                gameViewModel.Board = gameInfo.Board;
                gameViewModel.Name = gameInfo.Name;
                gameViewModel.ClubCity = gameInfo.ClubCity;
                gameViewModel.IsWhiteBlack = gameInfo.IsWhite;
                gameViewModel.Result = gameInfo.Result;

                GameList.Games.Add(gameViewModel);
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
            UseCache = false;
            IsLoading = false;
        }
    }
}