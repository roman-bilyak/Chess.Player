using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentGameViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int? _tournamentId;

    [ObservableProperty]
    private string? _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RoundName))]
    private int? _round;

    public string RoundName => $"R{Round}";

    [ObservableProperty]
    private int? _board;

    [ObservableProperty]
    private int? _no;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int? _rating;

    [ObservableProperty]
    private string? _clubCity;

    [ObservableProperty]
    private bool? _isWhiteBlack;

    public bool IsWhite=> IsWhiteBlack ?? false;

    public bool IsBlack => !IsWhiteBlack ?? false;

    [ObservableProperty]
    private double? _result;

    [ObservableProperty]
    private bool _isSelected;

    public PlayerTournamentGameViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ShowInfoAsync(CancellationToken cancellationToken)
    {
        if (!TournamentId.HasValue || !No.HasValue)
        {
            return;
        }

        IsSelected = true;

        await _navigationService.NavigateToPlayerTournamentAsync(TournamentId.Value, TournamentName, No.Value, Name);

        IsSelected = false;
    }
}