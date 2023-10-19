using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Views;

public partial class PlayerView : ContentPage
{
	public PlayerView(PlayerViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}