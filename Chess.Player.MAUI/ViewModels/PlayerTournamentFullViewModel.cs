using Chess.Player.Data;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentFullViewModel : BaseViewModel
{
    private readonly IChessDataService _chessDataService;

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    private string _tournamentLocation;

    [ObservableProperty]
    private int _playerStartingRank;

    [ObservableProperty]
    private string _playerName;

    [ObservableProperty]
    private double? _playerPoints;

    [ObservableProperty]
    private int? _playerRank;

    [ObservableProperty]
    private GameListViewModel _gameList;

    [ObservableProperty]
    private bool _forceRefresh;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    [ObservableProperty]
    private bool _isLoading = false;

    public PlayerTournamentFullViewModel
    (
        IChessDataService chessDataService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _chessDataService = chessDataService;
        _gameList = new GameListViewModel(navigationService);
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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            PlayerTournamentInfo playerTournamentInfo = await _chessDataService.GetPlayerTournamentInfoAsync(TournamentId, PlayerStartingRank, ForceRefresh, cancellationToken);

            TournamentName = playerTournamentInfo.Tournament.Name;
            TournamentLocation = playerTournamentInfo.Tournament.Location;

            PlayerName = playerTournamentInfo.Player.Name;
            PlayerPoints = playerTournamentInfo.Player.Points;
            PlayerRank = playerTournamentInfo.Player.Rank;

            GameList.Games.Clear();
            foreach(GameInfo gameInfo in playerTournamentInfo.Player.Games.OrderByDescending(x => x.Round))
            {
                GameList.Games.Add(new GameViewModel
                {
                    Round = gameInfo.Round,
                    Board = gameInfo.Board,
                    Name = gameInfo.Name,
                    ClubCity = gameInfo.ClubCity,
                    IsWhiteBlack = gameInfo.IsWhite,
                    Result = gameInfo.Result
                });
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
            IsLoading = false;
            ForceRefresh = true;
        }
    }
}