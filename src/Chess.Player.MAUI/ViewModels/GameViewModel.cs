using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class GameViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int? _tournamentId;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RoundName))]
    private int? _round;

    public string RoundName => $"R{Round}";

    [ObservableProperty]
    private int? _board;

    [ObservableProperty]
    private int? _no;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _clubCity;

    [ObservableProperty]
    private bool? _isWhiteBlack;

    public bool IsWhite=> IsWhiteBlack ?? false;

    public bool IsBlack => !IsWhiteBlack ?? false;

    [ObservableProperty]
    private double? _result;

    [ObservableProperty]
    private bool _isSelected;

    public GameViewModel
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

        await _navigationService.PushAsync<PlayerTournamentFullPage, PlayerTournamentFullViewModel>(x =>
        {
            x.TournamentId = TournamentId.Value;
            x.TournamentName = TournamentName;
            x.PlayerNo = No.Value;
            x.PlayerName = Name;
        });

        IsSelected = false;
    }
}