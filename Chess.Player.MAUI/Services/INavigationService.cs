namespace Chess.Player.Services
{
    public interface INavigationService
    {
        Task PushAsync<TPage, TViewModel>(Action<TViewModel> init = null)
            where TPage : Page;

        Task PopAsync();
    }
}