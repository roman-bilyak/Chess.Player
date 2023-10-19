using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class TournamentYearViewModel : BaseViewModel
{
    [ObservableProperty]
    private int? _year;

    [ObservableProperty]
    private int _years;

    [ObservableProperty]
    private int _count;
}