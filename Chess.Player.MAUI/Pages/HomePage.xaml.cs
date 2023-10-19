using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel searchViewModel)
	{
		InitializeComponent();

		BindingContext = searchViewModel;
	}
}