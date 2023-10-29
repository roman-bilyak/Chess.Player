using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentViewModel : BaseViewModel
{
    private readonly IDateTimeProvider _dateTimeProvider;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoAndTournamentName))]
    private int _tournamentNo;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoAndTournamentName))]
    private string _tournamentName;

    public string NoAndTournamentName => $"{TournamentNo}. {TournamentName}";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation), nameof(IsOnline), nameof(IsFuture))]
    private DateTime? _tournamentStartDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation), nameof(IsOnline), nameof(IsFuture))]
    private DateTime? _tournamentEndDate;

    public bool IsOnline
    {
        get
        {
            DateTime currentDate = _dateTimeProvider.UtcNow.Date;

            return (!TournamentStartDate.HasValue || currentDate >= TournamentStartDate.Value)
                && (!TournamentEndDate.HasValue || currentDate <= TournamentEndDate.Value);
        }
    }

    public bool IsFuture
    {
        get
        {
            DateTime currentDate = _dateTimeProvider.UtcNow.Date;
            return TournamentStartDate.HasValue && currentDate < TournamentStartDate.Value;
        }
    }

    public bool IsActive => IsOnline || IsFuture;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private string _tournamentLocation;

    public string TournamentDateAndLocation => TournamentStartDate == TournamentEndDate
        ? $"{TournamentEndDate:dd.MM} - {TournamentLocation}"
        : $"{TournamentStartDate:dd.MM} - {TournamentEndDate:dd.MM} - {TournamentLocation}";

    [ObservableProperty]
    private int? _numberOfPlayers;

    [ObservableProperty]
    private int? _numberOfRounds;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPodium))]
    private int? _rank;

    [ObservableProperty]
    private double? _points;

    public bool IsPodium => !IsActive && Rank.HasValue && Rank >= 1 && Rank <= 3;

    public PlayerTournamentViewModel
    (
        IDateTimeProvider dateTimeProvider
    )
    {
        ArgumentNullException.ThrowIfNull(dateTimeProvider);

        _dateTimeProvider = dateTimeProvider;
    }
}