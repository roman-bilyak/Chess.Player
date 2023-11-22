namespace Chess.Player.MAUI.Features.Home;

public partial class HomeView : ContentPage
{
	public HomeView(HomeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}