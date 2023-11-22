using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Features.Favorite;

public partial class FavoritesPage : ContentPage
{
	public FavoritesPage(FavoritesViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}