namespace Chess.Player.Services
{
    internal class MAUIPopupService : IPopupService
    {
        public async Task<string> DisplayPromptAsync(string title, string message, string accept, string cancel, string placeholder, int maxLength)
        {
            return await Shell.Current.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard: Keyboard.Text);
        }
    }
}