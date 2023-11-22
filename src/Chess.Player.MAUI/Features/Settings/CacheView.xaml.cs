namespace Chess.Player.MAUI.Features.Settings;

public partial class CacheView : ContentPage
{
    public CacheView(CacheViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}