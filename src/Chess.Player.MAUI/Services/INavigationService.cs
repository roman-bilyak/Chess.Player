using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.Services
{
    public interface INavigationService
    {
        Task PushAsync<TPage>()
            where TPage : Page;

        Task PushAsync<TPage, TViewModel>(Action<TViewModel> init)
            where TPage : Page
            where TViewModel : BaseViewModel;

        Task PopAsync();
    }
}