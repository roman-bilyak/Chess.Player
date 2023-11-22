namespace Chess.Player.MAUI.Services;

public interface ISettingsService
{
    event ThemeChangedEventHandler ThemeChanged;

    Task<AppTheme> GetThemeAsync(CancellationToken cancellationToken);

    Task SetThemeAsync(AppTheme theme, CancellationToken cancellationToken);
}