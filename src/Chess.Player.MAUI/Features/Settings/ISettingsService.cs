namespace Chess.Player.MAUI.Features.Settings;

public interface ISettingsService
{
    event ThemeChangedEventHandler ThemeChanged;

    Task<AppTheme> GetThemeAsync(CancellationToken cancellationToken);

    Task SetThemeAsync(AppTheme theme, CancellationToken cancellationToken);
}