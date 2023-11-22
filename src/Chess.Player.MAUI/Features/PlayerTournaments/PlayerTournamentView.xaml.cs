namespace Chess.Player.MAUI.Features.PlayerTournaments;

public partial class PlayerTournamentView : ContentPage
{
	public PlayerTournamentView(PlayerTournamentViewModel viewModel)
    {
		InitializeComponent();

        BindingContext = viewModel;
    }
}