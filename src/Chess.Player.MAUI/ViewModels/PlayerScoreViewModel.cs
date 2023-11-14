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
    private int? _rank;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _clubCity;

    [ObservableProperty]
    private double? _points;

    [ObservableProperty]
    private double? _tB1;

    [ObservableProperty]
    private double? _tB2;

    [ObservableProperty]
    private double? _tB3;

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
        IsSelected = true;

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = Name;
        });

        IsSelected = false;
    }
}