using Chess.Player.Data;
using Chess.Player.MAUI.Features.Players;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Favorites;

public partial class FavoritesViewModel : BaseRefreshViewModel, IDisposable
{
    private readonly IFavoriteService _favoriteService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableCollection<PlayerShortViewModel> _players = [];

    public FavoritesViewModel
    (
        IFavoriteService favoriteService,
        IDateTimeProvider dateTimeProvider,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(favoriteService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _favoriteService = favoriteService;
        _dateTimeProvider = dateTimeProvider;
        _serviceProvider = serviceProvider;

        _favoriteService.ProgressChanged += OnProgressChanged;
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        List<PlayerShortViewModel> players = [];
        DateTime currentDate = _dateTimeProvider.UtcNow.Date;
        foreach (PlayerFullInfo playerInfo in await _favoriteService.GetAllAsync(UseCache, cancellationToken))
        {
            PlayerShortViewModel player = _serviceProvider.GetRequiredService<PlayerShortViewModel>();

            player.Names = new ObservableCollection<PlayerNameViewModel>(playerInfo.Names.Select(x => new PlayerNameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
            player.Title = playerInfo.Title;
            player.ClubCity = playerInfo.ClubCity;
            player.YearOfBirth = playerInfo.YearOfBirth;
            player.HasOnlineTournaments = playerInfo.HasOnlineTournaments(currentDate);
            player.HasFutureTournaments = playerInfo.HasFutureTournaments(currentDate);

            players.Add(player);
        }

        Players.Clear();
        foreach (PlayerShortViewModel player in players)
        {
            Players.Add(player);
        }
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _favoriteService.ProgressChanged -= OnProgressChanged;
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FavoritesViewModel()
    {
        Dispose(false);
    }

    #region helper methods

    private void OnProgressChanged(object sender, ProgressEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Progress += (double)e.Percentage / 100;
        });
    }

    #endregion
}