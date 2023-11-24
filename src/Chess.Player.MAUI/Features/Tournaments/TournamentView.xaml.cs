namespace Chess.Player.MAUI.Features.Tournaments;

public partial class TournamentView : BaseRefreshView
{
	public TournamentView(TournamentViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}