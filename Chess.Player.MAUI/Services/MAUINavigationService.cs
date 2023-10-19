using Chess.Player.MAUI;

namespace Chess.Player.Services
{
    internal class MAUINavigationService : INavigationService
    {

        private readonly IServiceProvider _serviceProvider;

        public MAUINavigationService
        (
            IServiceProvider serviceProvider
        )
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            _serviceProvider = serviceProvider;
        }

        public async Task PushAsync<TPage, TViewModel>(Action<TViewModel> initViewModel)
            where TPage : Page
        {
            TViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            if (initViewModel is not null)
            {
                initViewModel(viewModel);
            }

            TPage page = _serviceProvider.GetRequiredService<TPage>();
            page.BindingContext = viewModel;

            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}