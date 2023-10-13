using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class PlayerViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _lastName;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        private string _firstName;

        public string FullName => $"{LastName} {FirstName}";

        [RelayCommand]
        public async Task CancelAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
