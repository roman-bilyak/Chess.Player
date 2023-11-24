namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentView : BaseRefreshView
{
    public PlayerTournamentView(PlayerTournamentViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}