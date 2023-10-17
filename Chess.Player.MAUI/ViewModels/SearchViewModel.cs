using Chess.Player.MAUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

        [ObservableProperty]
        private RecentPlayerViewModel _selectedPlayer;

        public SearchViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task GoAsync()
        {
            string[] searchParts = SearchText.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            await NavigateToPlayerViewAsync(searchParts[0], string.Join(" ", searchParts.Skip(1)));
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(RecentPlayerViewModel selectedPlayer)
        {
            if (selectedPlayer == null)
            {
                return;
            }
            await NavigateToPlayerViewAsync(selectedPlayer.LastName, selectedPlayer.FirstName);
        }

        private async Task NavigateToPlayerViewAsync(string lastName, string firstName)
        {
            PlayerViewModel playerViewModel = _serviceProvider.GetService<PlayerViewModel>();
            playerViewModel.LastName = lastName;
            playerViewModel.FirstName = firstName;

            PlayerView playerView = new() { BindingContext = playerViewModel };

            await App.Current.MainPage.Navigation.PushAsync(playerView);

            SelectedPlayer = null;
        }
    }
}