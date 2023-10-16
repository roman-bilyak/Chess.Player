using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels
{
    public partial class RecentPlayerViewModel: ObservableObject
    {
        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _firstName;
    }
}