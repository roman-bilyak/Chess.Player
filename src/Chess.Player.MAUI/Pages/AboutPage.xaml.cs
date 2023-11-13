using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel viewModel)
    {
		InitializeComponent();

        BindingContext = viewModel;
    }
}