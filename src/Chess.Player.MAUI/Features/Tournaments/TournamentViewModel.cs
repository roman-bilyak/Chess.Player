using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Tournaments;

public partial class TournamentViewModel : BaseRefreshViewModel
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
    private ObservableCollection<TournamentPlayerScoreViewModel> _playerScores = [];

    public TournamentViewModel
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

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        TournamentInfo tournamentInfo = await _chessDataService.GetTournamentInfoAsync(TournamentId, UseCache, cancellationToken);

        TournamentName = tournamentInfo.Name;
        TournamentEndDate = tournamentInfo.EndDate;
        TournamentLocation = tournamentInfo.Location;

        PlayerScores.Clear();
        foreach (PlayerScoreInfo playerScoreInfo in tournamentInfo.Players)
        {
            TournamentPlayerScoreViewModel playerScoreViewModel = _serviceProvider.GetRequiredService<TournamentPlayerScoreViewModel>();

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
    }
}