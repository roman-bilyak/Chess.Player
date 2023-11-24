namespace Chess.Player.MAUI.Features.Home;

public partial class HomeView : BaseRefreshView
{
    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}