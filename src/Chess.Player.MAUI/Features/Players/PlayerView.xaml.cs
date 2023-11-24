namespace Chess.Player.MAUI.Features.Players;

public partial class PlayerView : BaseRefreshView
{
	public PlayerView(PlayerViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}