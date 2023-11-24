namespace Chess.Player.MAUI.Features.Players;

public partial class PlayerView : BaseRefreshView
{
	public PlayerView(PlayerViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}