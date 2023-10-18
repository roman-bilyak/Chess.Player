using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels
{
    internal partial class TournamentYearViewModel : ObservableObject
    {
        [ObservableProperty]
        private int? _year;

        [ObservableProperty]
        private int _years;

        [ObservableProperty]
        private int _count;
    }
}