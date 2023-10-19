using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel searchViewModel)
	{
		InitializeComponent();

		BindingContext = searchViewModel;
	}
}