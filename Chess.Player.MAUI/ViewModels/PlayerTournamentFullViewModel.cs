using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentFullViewModel: BaseViewModel
{
    [ObservableProperty]
    private string _playerStartingRank;

    [ObservableProperty]
    private string _playerName;

    [ObservableProperty]
    private string _tournamentNo;

    [ObservableProperty]
    private string _tournamentName;
}