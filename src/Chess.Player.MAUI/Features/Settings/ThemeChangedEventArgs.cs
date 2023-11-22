namespace Chess.Player.MAUI.Services;

public class ThemeChangedEventArgs : EventArgs
{
    public AppTheme Theme { get; }

    public ThemeChangedEventArgs(AppTheme theme)
    {
        Theme = theme;
    }
}