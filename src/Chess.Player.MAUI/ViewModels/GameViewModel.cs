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
    [NotifyPropertyChangedFor(nameof(RoundName))]
    private int? _round;

    public string RoundName => $"R{Round}";

    [ObservableProperty]
    private int? _board;

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
        IsSelected = true;

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = Name;
        });

        IsSelected = false;
    }
}