using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerScoreListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<PlayerScoreViewModel> _playerScores = new();
}
