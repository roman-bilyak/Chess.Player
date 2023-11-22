namespace Chess.Player.MAUI.Features.Players;

public partial class PlayerView : ContentPage
{
	public PlayerView(PlayerViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}