using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class GameViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RoundName))]
    private int? _round;

    public string RoundName => $"R{Round}";

    [ObservableProperty]
    private int? _board;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _clubCity;

    [ObservableProperty]
    private bool? _isWhiteBlack;

    public bool IsWhite=> IsWhiteBlack ?? false;

    public bool IsBlack => !IsWhiteBlack ?? false;

    [ObservableProperty]
    private double? _result;
}