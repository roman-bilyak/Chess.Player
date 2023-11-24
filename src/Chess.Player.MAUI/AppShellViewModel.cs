using Chess.Player.MAUI.Features.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI;

public partial class AppShellViewModel : ObservableObject
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
        if (Application.Current is null)
        {
            return;
        }

        Application.Current.UserAppTheme = await _settingsService.GetThemeAsync(cancellationToken);
    }
}