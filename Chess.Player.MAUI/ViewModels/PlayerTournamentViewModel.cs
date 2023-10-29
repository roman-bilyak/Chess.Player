using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentViewModel : BaseViewModel
{
    private readonly DateTime _currentDate;

    [ObservableProperty]
    private int _tournamentNo;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation), nameof(IsOnline), nameof(IsFuture), nameof(IsNotFuture), nameof(IsActive), nameof(IsPodium))]
    private DateTime? _tournamentStartDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation), nameof(IsOnline), nameof(IsFuture), nameof(IsNotFuture), nameof(IsActive), nameof(IsPodium))]
    private DateTime? _tournamentEndDate;

    public bool IsOnline
    {
        get
        {
            return (!TournamentStartDate.HasValue || _currentDate >= TournamentStartDate.Value)
                && (!TournamentEndDate.HasValue || _currentDate <= TournamentEndDate.Value);
        }
    }

    public bool IsFuture
    {
        get
        {
            return TournamentStartDate.HasValue && _currentDate < TournamentStartDate.Value;
        }
    }

    public bool IsNotFuture => !IsFuture;

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

        _currentDate = dateTimeProvider.UtcNow.Date;
    }
}