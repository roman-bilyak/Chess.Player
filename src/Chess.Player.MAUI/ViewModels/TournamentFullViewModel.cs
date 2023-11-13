using Chess.Player.Data;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class TournamentFullViewModel : BaseViewModel
{
    private readonly IChessDataService _chessDataService;

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    private DateTime? _tournamentEndDate;

    [ObservableProperty]
    private string _tournamentLocation;

    [ObservableProperty]
    private PlayerScoreListViewModel _playerScoreList;

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public TournamentFullViewModel
    (
        IChessDataService chessDataService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _chessDataService = chessDataService;

        _playerScoreList = new PlayerScoreListViewModel(navigationService);
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
            TournamentInfo tournamentInfo = await _chessDataService.GetTournamentInfoAsync(TournamentId, UseCache, cancellationToken);

            TournamentName = tournamentInfo.Name;
            TournamentEndDate = tournamentInfo.EndDate;
            TournamentLocation = tournamentInfo.Location;

            PlayerScoreList.PlayerScores.Clear();
            foreach (PlayerScoreInfo playerScoreInfo in tournamentInfo.Players)
            {
                PlayerScoreList.PlayerScores.Add(new PlayerScoreViewModel
                {
                    Rank = playerScoreInfo.Rank,
                    Name = playerScoreInfo.Name,
                    ClubCity = playerScoreInfo.ClubCity,
                    Points = playerScoreInfo.Points,
                    TB1 = playerScoreInfo.TB1,
                    TB2 = playerScoreInfo.TB2,
                    TB3 = playerScoreInfo.TB3,
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
            UseCache = false;
            IsLoading = false;
        }
    }
}