using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerScoreViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int? _tournametId;

    [ObservableProperty]
    private string? _tournamentName;

    [ObservableProperty]
    private int? _rank;

    [ObservableProperty]
    private int? _no;

    [ObservableProperty]
    private string? _name;

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

    public PlayerScoreViewModel
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
        if (!TournametId.HasValue || !No.HasValue)
        {
            return;
        }

        IsSelected = true;

        await _navigationService.PushAsync<PlayerTournamentFullPage, PlayerTournamentFullViewModel>(x =>
        {
            x.TournamentId = TournametId.Value;
            x.TournamentName = TournamentName;
            x.PlayerNo = No.Value;
            x.PlayerName = Name;
        });

        IsSelected = false;
    }

    [RelayCommand]
    private async Task ShowPlayerInfoAsync(CancellationToken cancellationToken)
    {
        IsSelected = true;

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = Name;
        });

        IsSelected = false;
    }
}