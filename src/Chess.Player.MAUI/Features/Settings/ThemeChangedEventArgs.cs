namespace Chess.Player.MAUI.Features.Settings;

public class ThemeChangedEventArgs(AppTheme theme) : EventArgs
{
    public AppTheme Theme { get; } = theme;
}