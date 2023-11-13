using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerScoreListViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<PlayerScoreViewModel> _playerScores = new();

    [ObservableProperty]
    private PlayerScoreViewModel _selectedPlayerScore;

    public PlayerScoreListViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(PlayerScoreViewModel selectedGame)
    {
        if (selectedGame == null)
        {
            return;
        }

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = selectedGame.Name;
        });

        SelectedPlayerScore = null;
    }
}
