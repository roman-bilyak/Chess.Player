using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class GameListViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<GameViewModel> _games = new();

    [ObservableProperty]
    private GameViewModel _selectedGame;

    public GameListViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(GameViewModel selectedGame)
    {
        if (selectedGame == null)
        {
            return;
        }

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = selectedGame.Name;
        });

        SelectedGame = null;
    }
}
