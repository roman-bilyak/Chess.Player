using Chess.Player.Data;
using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerViewModel : BaseViewModel
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INavigationService _navigationService;

    public string Name => Names.FirstOrDefault()?.FullName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    private ObservableCollection<NameViewModel> _names = new();

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasClubCity))]
    private string _clubCity;

    public bool HasClubCity => !string.IsNullOrEmpty(ClubCity);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Years), nameof(HasYearOfBirth))]
    private int? _yearOfBirth;

    public bool HasYearOfBirth => YearOfBirth is not null;

    public int Years => _dateTimeProvider.UtcNow.Year - YearOfBirth ?? 0;

    [ObservableProperty]
    private bool _hasOnlineTournaments;

    [ObservableProperty]
    private bool _hasFutureTournaments;

    [ObservableProperty]
    private bool _isSelected;

    public PlayerViewModel
    (
        IDateTimeProvider dateTimeProvider,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(navigationService);

        _dateTimeProvider = dateTimeProvider;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ShowInfoAsync(CancellationToken cancellationToken)
    {
        IsSelected = true;

        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = Name;
        });

        IsSelected = false;
    }
}