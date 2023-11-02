using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerListViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<PlayerViewModel> _players = new();

    [ObservableProperty]
    private PlayerViewModel _selectedPlayer;

    public PlayerListViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(PlayerViewModel selectedPlayer)
    {
        if (selectedPlayer == null)
        {
            return;
        }

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = selectedPlayer.Name;
        });

        SelectedPlayer = null;
    }
}