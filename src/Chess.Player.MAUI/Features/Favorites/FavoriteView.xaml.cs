using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Features.Favorites;

public partial class FavoritesView : ContentPage
{
	public FavoritesView(FavoritesViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}