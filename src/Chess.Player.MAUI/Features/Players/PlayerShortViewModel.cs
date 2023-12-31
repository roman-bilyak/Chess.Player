﻿using Chess.Player.Data;
using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Players;

public partial class PlayerShortViewModel : BaseViewModel
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INavigationService _navigationService;

    public string? Name => Names.FirstOrDefault()?.FullName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    private ObservableCollection<PlayerNameViewModel> _names = [];

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasClubCity))]
    private string? _clubCity;

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

    public PlayerShortViewModel
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
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        IsSelected = true;

        await _navigationService.NavigateToPlayerAsync(Name);

        IsSelected = false;
    }
}