using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class GameListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<GameViewModel> _games = new();
}