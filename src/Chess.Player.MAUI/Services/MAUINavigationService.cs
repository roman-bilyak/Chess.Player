using Chess.Player.MAUI.ViewModels;

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

        public async Task PushAsync<TPage>() where TPage : Page
        {
            TPage page = _serviceProvider.GetRequiredService<TPage>();
            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task PushAsync<TPage, TViewModel>(Action<TViewModel> init)
            where TPage : Page
            where TViewModel: BaseViewModel
        {
            TPage page = _serviceProvider.GetRequiredService<TPage>();

            TViewModel viewModel = (TViewModel)page.BindingContext;
            if (init is not null && viewModel is not null)
            {
                init(viewModel);
            }

            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}