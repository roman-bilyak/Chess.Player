namespace Chess.Player.MAUI.Navigation;

public interface IPopupService
{
    Task<string> DisplayPromptAsync(string title, string? message = null, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1);
}