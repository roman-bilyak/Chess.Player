using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class TournamentFullPage : ContentPage
{
	public TournamentFullPage(TournamentFullViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}