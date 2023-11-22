using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.Features.Players;

[INotifyPropertyChanged]
public partial class PlayerNameViewModel: BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string? _lastName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string? _firstName;

    public string FullName => $"{LastName} {FirstName}";
}
