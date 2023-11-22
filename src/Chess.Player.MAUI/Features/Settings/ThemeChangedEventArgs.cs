namespace Chess.Player.MAUI.Features.Settings;

public class ThemeChangedEventArgs : EventArgs
{
    public AppTheme Theme { get; }

    public ThemeChangedEventArgs(AppTheme theme)
    {
        Theme = theme;
    }
}