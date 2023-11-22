namespace Chess.Player.MAUI.Features.Settings;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}