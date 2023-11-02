namespace Chess.Player.Services
{
    internal class MAUIPopupService : IPopupService
    {
        public async Task<string> DisplayPromptAsync(string title, string message, string accept, string cancel, string placeholder, int maxLength)
        {
            Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
            return await page.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard: Keyboard.Text);
        }
    }
}
