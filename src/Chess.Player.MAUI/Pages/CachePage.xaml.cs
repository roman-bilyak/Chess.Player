using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class CachePage : ContentPage
{
    public CachePage(CacheViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}