namespace Chess.Player.MAUI.Features.Favorites;

public partial class FavoritesView : BaseRefreshView
{
    public FavoritesView(FavoritesViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}