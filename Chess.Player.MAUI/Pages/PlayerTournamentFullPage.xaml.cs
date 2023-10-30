using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class PlayerTournamentFullPage : ContentPage
{
	public PlayerTournamentFullPage(PlayerTournamentFullViewModel viewModel)
    {
		InitializeComponent();

        BindingContext = viewModel;
    }
}