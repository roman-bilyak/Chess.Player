﻿using Chess.Player.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class AppShellViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;

    public AppShellViewModel
    (
        ISettingsService settingsService
    )
    {
        ArgumentNullException.ThrowIfNull(settingsService);

        _settingsService = settingsService;
    }


    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadThemeAsync(CancellationToken cancellationToken)
    {
        Application.Current.UserAppTheme = await _settingsService.GetThemeAsync(cancellationToken);
    }
}