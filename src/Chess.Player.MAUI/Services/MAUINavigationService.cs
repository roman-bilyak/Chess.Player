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

        public async Task PushAsync<TPage, TViewModel>(Action<TViewModel> init)
            where TPage : Page
        {
            TPage page = _serviceProvider.GetRequiredService<TPage>();

            TViewModel viewModel = (TViewModel)page.BindingContext;
            if (init is not null && viewModel is not null)
            {
                init(viewModel);
            }

            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}