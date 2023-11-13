using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerScoreViewModel : BaseViewModel
{
    [ObservableProperty]
    private int? _rank;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _clubCity;

    [ObservableProperty]
    private double? _points;

    [ObservableProperty]
    private double? _tB1;

    [ObservableProperty]
    private double? _tB2;

    [ObservableProperty]
    private double? _tB3;
}