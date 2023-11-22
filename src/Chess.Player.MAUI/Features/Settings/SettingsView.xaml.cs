namespace Chess.Player.MAUI.Features.Settings;

public partial class SettingsView : ContentPage
{
	public SettingsView(SettingsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}