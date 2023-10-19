namespace Chess.Player.Services
{
    public interface INavigationService
    {
        Task PushAsync<TPage, TViewModel>(Action<TViewModel> initViewModel = null)
            where TPage : Page;

        Task PopAsync();
    }
}