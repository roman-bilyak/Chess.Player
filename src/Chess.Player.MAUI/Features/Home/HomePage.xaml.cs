namespace Chess.Player.MAUI.Features.Home;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}