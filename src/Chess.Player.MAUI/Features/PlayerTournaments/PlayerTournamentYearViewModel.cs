using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.Features.PlayerTournaments;

[INotifyPropertyChanged]
public partial class PlayerTournamentYearViewModel : BaseViewModel
{
    [ObservableProperty]
    private int? _year;

    [ObservableProperty]
    private int _years;

    [ObservableProperty]
    private int _count;
}