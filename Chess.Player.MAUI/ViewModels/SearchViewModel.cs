using Chess.Player.MAUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class SearchViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required]
        private string _lastName;

        [ObservableProperty]
        [Required]
        private string _firstName;

        [ObservableProperty]
        private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

        [ObservableProperty]
        private RecentPlayerViewModel _selectedPlayer;

        [RelayCommand]
        private async Task GoAsync()
        {
            await NavigateToPlayerViewAsync(LastName, FirstName);

            RecentPlayers.Insert(0, new RecentPlayerViewModel
            {
                LastName = LastName,
                FirstName = FirstName
            });
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
            PlayerViewModel playerViewModel = new()
            {
                LastName = lastName,
                FirstName = firstName,
            };
            PlayerView playerView = new() { BindingContext = playerViewModel };

            await App.Current.MainPage.Navigation.PushAsync(playerView);

            SelectedPlayer = null;
        }
    }
}