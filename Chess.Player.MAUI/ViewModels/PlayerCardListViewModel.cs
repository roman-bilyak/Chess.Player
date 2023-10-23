using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerCardListViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<PlayerCardViewModel> _players = new();

    [ObservableProperty]
    private PlayerCardViewModel _selectedPlayer;

    public PlayerCardListViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(PlayerCardViewModel selectedPlayer)
    {
        if (selectedPlayer == null)
        {
            return;
        }

        await _navigationService.PushAsync<PlayerPage, PlayerViewModel>(x =>
        {
            x.SearchCriterias.Add(selectedPlayer.FullName);
        });

        SelectedPlayer = null;
    }
}