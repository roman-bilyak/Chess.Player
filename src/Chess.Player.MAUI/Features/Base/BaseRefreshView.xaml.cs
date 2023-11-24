namespace Chess.Player.MAUI.Features;

public partial class BaseRefreshView : ContentPage
{
    public BaseRefreshView(BaseRefreshViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}