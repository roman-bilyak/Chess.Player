namespace Chess.Player.MAUI.Features.Tournaments;

public partial class TournamentView : ContentPage
{
	public TournamentView(TournamentViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}