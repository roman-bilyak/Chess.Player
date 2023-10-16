using Chess.Player.MAUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Chess.Player.MAUI.ViewModels
{
    public partial class SearchViewModel : ObservableValidator
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        [Required]
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
            string[] searchParts = SearchText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (searchParts.Length > 1)
            {
                await NavigateToPlayerViewAsync(searchParts[0], string.Join(" ", searchParts.Skip(1)));
            }
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