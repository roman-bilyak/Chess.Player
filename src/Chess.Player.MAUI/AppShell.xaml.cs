using Chess.Player.MAUI.ViewModels;

namespace Chess.Player.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}