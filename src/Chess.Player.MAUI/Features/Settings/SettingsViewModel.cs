using Chess.Player.MAUI.Features;
using Chess.Player.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private AppTheme _theme;

    [ObservableProperty]
    private ObservableCollection<AppTheme> _themes = new(new[] { AppTheme.Unspecified, AppTheme.Light, AppTheme.Dark });

    public SettingsViewModel
    (
        ISettingsService settingsService
    )
    {
        ArgumentNullException.ThrowIfNull(settingsService);

        _settingsService = settingsService;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadSettingsAsync(CancellationToken cancellationToken)
    {
        Theme = await _settingsService.GetThemeAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task SaveSettingsAsync(CancellationToken cancellationToken)
    {
        await _settingsService.SetThemeAsync(Theme, cancellationToken);
    }
}