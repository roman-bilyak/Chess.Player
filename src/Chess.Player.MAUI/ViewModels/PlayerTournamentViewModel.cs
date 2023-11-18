﻿using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class PlayerTournamentViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int? _tournamentId;

    [ObservableProperty]
    private int? _tournamentIndex;

    [ObservableProperty]
    private string _tournamentName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private DateTime? _tournamentStartDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private DateTime? _tournamentEndDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsActive), nameof(IsPodium))]
    private bool _isOnline;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsActive), nameof(IsPodium))]
    private bool _isFuture;

    public bool IsNotFuture => !IsFuture;

    public bool IsActive => IsOnline || IsFuture;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TournamentDateAndLocation))]
    private string _tournamentLocation;

    public string TournamentDateAndLocation => TournamentStartDate == TournamentEndDate
        ? $"{TournamentEndDate:dd.MM} - {TournamentLocation}"
        : $"{TournamentStartDate:dd.MM} - {TournamentEndDate:dd.MM} - {TournamentLocation}";

    [ObservableProperty]
    private int? _numberOfPlayers;

    [ObservableProperty]
    private int? _numberOfRounds;

    [ObservableProperty]
    private int? _no;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPodium))]
    private int? _rank;

    [ObservableProperty]
    private double? _points;

    public bool IsPodium => !IsActive && Rank.HasValue && Rank >= 1 && Rank <= 3;

    public bool ShowFullInfo { get; set; }

    [ObservableProperty]
    private bool _isSelected;

    public PlayerTournamentViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ShowInfoAsync(CancellationToken cancellationToken)
    {
        if (!TournamentId.HasValue || !No.HasValue)
        {
            return;
        }

        IsSelected = true;

        if (ShowFullInfo)
        {
            await _navigationService.PushAsync<TournamentFullPage, TournamentFullViewModel>(x =>
            {
                x.TournamentId = TournamentId.Value;
                x.TournamentName = TournamentName;
            });
        }
        else
        {
            await _navigationService.PushAsync<PlayerTournamentFullPage, PlayerTournamentFullViewModel>(x =>
            {
                x.TournamentId = TournamentId.Value;
                x.TournamentName = TournamentName;

                x.PlayerNo = No.Value;
                x.PlayerName = Name;
            });
        }

        IsSelected = false;
    }
}