using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<PlayerViewModel> _players = new();
}