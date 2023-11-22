using Chess.Player.Data;
using Chess.Player.MAUI.Features.Players;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.Features.Favorites;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableCollection<PlayerShortViewModel> _players = [];

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public FavoritesViewModel
    (
        IPlayerFavoriteService playerFavoriteService,
        IDateTimeProvider dateTimeProvider,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _playerFavoriteService = playerFavoriteService;
        _dateTimeProvider = dateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private Task StartAsync(CancellationToken cancellationToken)
    {
        UseCache = true;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        UseCache = false;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<PlayerShortViewModel> players = [];
            DateTime currentDate = _dateTimeProvider.UtcNow.Date;
            foreach (PlayerFullInfo player in await _playerFavoriteService.GetAllAsync(UseCache, cancellationToken))
            {
                PlayerShortViewModel playerViewModel = _serviceProvider.GetRequiredService<PlayerShortViewModel>();

                playerViewModel.Names = new ObservableCollection<PlayerNameViewModel>(player.Names.Select(x => new PlayerNameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
                playerViewModel.Title = player.Title;
                playerViewModel.ClubCity = player.ClubCity;
                playerViewModel.YearOfBirth = player.YearOfBirth;
                playerViewModel.HasOnlineTournaments = player.HasOnlineTournaments(currentDate);
                playerViewModel.HasFutureTournaments = player.HasFutureTournaments(currentDate);

                players.Add(playerViewModel);
            }

            Players.Clear();
            foreach (PlayerShortViewModel playerViewModel in players)
            {
                Players.Add(playerViewModel);
            }

            Error = null;
        }
        catch (OperationCanceledException)
        {

        }
        catch (WebException)
        {
            Error = "No internet connection.";
        }
        catch
        {
            Error = "Oops! Something went wrong.";
        }
        finally
        {
            UseCache = false;
            IsLoading = false;
        }
    }
}