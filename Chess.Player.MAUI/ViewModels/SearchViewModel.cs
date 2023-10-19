using Chess.Player.MAUI.Views;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

        [ObservableProperty]
        private RecentPlayerViewModel _selectedPlayer;

        public SearchViewModel
        (
            INavigationService navigationService
        )
        {
            ArgumentNullException.ThrowIfNull(navigationService);

            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task GoAsync()
        {
            await NavigateToPlayerViewAsync(SearchText);
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(RecentPlayerViewModel selectedPlayer)
        {
            if (selectedPlayer == null)
            {
                return;
            }
            await NavigateToPlayerViewAsync($"{selectedPlayer.LastName} {selectedPlayer.FirstName}");
        }

        private async Task NavigateToPlayerViewAsync(string name)
        {
            await _navigationService.PushAsync<PlayerView, PlayerViewModel>(x =>
            {
                x.SearchCriterias.Add(name);
            });

            SelectedPlayer = null;
        }
    }
}