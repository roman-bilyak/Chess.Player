using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.Features.Tournaments;

public partial class TournamentPlayerScoreViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int? _tournamentId;

    [ObservableProperty]
    private string? _tournamentName;

    [ObservableProperty]
    private int? _rank;

    [ObservableProperty]
    private int? _no;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int? _rating;

    [ObservableProperty]
    private string? _clubCity;

    [ObservableProperty]
    private double? _points;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TB1Format))]
    private double? _tB1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TB2Format))]
    private double? _tB2;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TB3Format))]
    private double? _tB3;

    public string TB1Format => $"{TB1:F01}";

    public string TB2Format => $"{TB2:F01}";

    public string TB3Format => $"{TB3:F01}";

    [ObservableProperty]
    private bool _isSelected;

    public TournamentPlayerScoreViewModel
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