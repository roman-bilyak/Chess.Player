namespace Chess.Player.MAUI.Features.Info;

public partial class InfoView : ContentPage
{
	public InfoView(InfoViewModel viewModel)
    {
		InitializeComponent();

        BindingContext = viewModel;
    }
}