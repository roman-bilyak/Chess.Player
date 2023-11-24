namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentView : BaseRefreshView
{
    public PlayerTournamentView(PlayerTournamentViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}