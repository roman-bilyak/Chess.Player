using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class NameViewModel: BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string? _lastName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    private string? _firstName;

    public string FullName => $"{LastName} {FirstName}";
}
