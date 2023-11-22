using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class PlayerFullPage : ContentPage
{
	public PlayerFullPage(PlayerFullViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}