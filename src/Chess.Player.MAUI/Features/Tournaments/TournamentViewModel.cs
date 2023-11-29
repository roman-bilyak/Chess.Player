using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Tournaments;

public partial class TournamentViewModel : BaseRefreshViewModel
{
    private readonly IChessDataService _chessDataService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private int _tournamentId;

    [ObservableProperty]
    private string? _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentStartDateStr))]
    private DateTime? _tournamentStartDate;

    public string? TournamentStartDateStr => TournamentStartDate?.ToString("dd.MM.yy");

    [ObservableProperty]
    private DateTime? _tournamentEndDate;

    public string? TournamentEndDateStr => TournamentEndDate?.ToString("dd.MM.yy");

    [ObservableProperty]
    private string? _tournamentLocation;

    [ObservableProperty]
    private int? _tournamentNumberOfPlayers;

    [ObservableProperty]
    private int? _tournamentNumberOfRounds;

    [ObservableProperty]
    private bool _tournamentIsOnline;

    [ObservableProperty]
    private bool _tournamentIsFuture;

    [ObservableProperty]
    private ObservableCollection<TournamentPlayerScoreViewModel> _playerScores = [];

    public TournamentViewModel
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
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        DateTime currentDate = _dateTimeProvider.UtcNow.Date;

        TournamentInfo tournamentInfo = await _chessDataService.GetTournamentInfoAsync(TournamentId, UseCache, cancellationToken);

        TournamentName = tournamentInfo.Name;
        TournamentStartDate = tournamentInfo.StartDate;
        TournamentEndDate = tournamentInfo.EndDate;
        TournamentLocation = tournamentInfo.Location;
        TournamentNumberOfPlayers = tournamentInfo.Players.Count;
        TournamentNumberOfRounds = tournamentInfo.NumberOfRounds;
        TournamentIsOnline = tournamentInfo.IsOnline(currentDate);
        TournamentIsFuture = tournamentInfo.IsFuture(currentDate);

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