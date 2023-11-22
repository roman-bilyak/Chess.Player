namespace Chess.Player.MAUI.Features.Settings;

public partial class CachePage : ContentPage
{
    public CachePage(CacheViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}