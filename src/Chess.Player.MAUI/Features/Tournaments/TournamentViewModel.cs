using Chess.Player.Data;
using Chess.Player.MAUI.Features;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class TournamentFullViewModel : BaseViewModel
{
    private readonly IChessDataService _chessDataService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    private string? _tournamentName;

    [ObservableProperty]
    private DateTime? _tournamentEndDate;

    [ObservableProperty]
    private string? _tournamentLocation;

    [ObservableProperty]
    private ObservableCollection<PlayerScoreViewModel> _playerScores = [];

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public TournamentFullViewModel
    (
        IChessDataService chessDataService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _chessDataService = chessDataService;
        _serviceProvider = serviceProvider;
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

            PlayerScores.Clear();
            foreach (PlayerScoreInfo playerScoreInfo in tournamentInfo.Players)
            {
                PlayerScoreViewModel playerScoreViewModel = _serviceProvider.GetRequiredService<PlayerScoreViewModel>();

                playerScoreViewModel.TournamentId = TournamentId;
                playerScoreViewModel.TournamentName = TournamentName;
                playerScoreViewModel.Rank = playerScoreInfo.Rank;
                playerScoreViewModel.No = playerScoreInfo.No;
                playerScoreViewModel.Name = playerScoreInfo.Name;
                playerScoreViewModel.ClubCity = playerScoreInfo.ClubCity;
                playerScoreViewModel.Points = playerScoreInfo.Points;
                playerScoreViewModel.TB1 = playerScoreInfo.TB1;
                playerScoreViewModel.TB2 = playerScoreInfo.TB2;
                playerScoreViewModel.TB3 = playerScoreInfo.TB3;

                PlayerScores.Add(playerScoreViewModel);
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