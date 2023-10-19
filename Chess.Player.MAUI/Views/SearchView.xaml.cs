using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Views;

public partial class SearchView : ContentPage
{
	public SearchView(SearchViewModel searchViewModel)
	{
		InitializeComponent();

		BindingContext = searchViewModel;
	}
}