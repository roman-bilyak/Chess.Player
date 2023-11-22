using Chess.Player.Data;
using Chess.Player.MAUI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.Features.Home;

[INotifyPropertyChanged]
public partial class HomeViewModel : BaseViewModel
{
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly INavigationService _navigationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private ObservableCollection<PlayerViewModel> _players = [];

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public HomeViewModel
    (
        IPlayerHistoryService playerHistoryService,
        INavigationService navigationService,
        IDateTimeProvider dateTimeProvider,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(navigationService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _playerHistoryService = playerHistoryService;
        _navigationService = navigationService;
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

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<PlayerViewModel> players = [];
            DateTime currentDate = _dateTimeProvider.UtcNow.Date;
            foreach (PlayerFullInfo player in await _playerHistoryService.GetAllAsync(UseCache, cancellationToken))
            {
                PlayerViewModel playerViewModel = _serviceProvider.GetRequiredService<PlayerViewModel>();

                playerViewModel.Names = new ObservableCollection<NameViewModel>(player.Names.Select(x => new NameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
                playerViewModel.Title = player.Title;
                playerViewModel.ClubCity = player.ClubCity;
                playerViewModel.YearOfBirth = player.YearOfBirth;
                playerViewModel.HasOnlineTournaments = player.HasOnlineTournaments(currentDate);
                playerViewModel.HasFutureTournaments = player.HasFutureTournaments(currentDate);

                players.Add(playerViewModel);
            }

            Players.Clear();
            foreach(PlayerViewModel playerViewModel in players)
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

    [RelayCommand]
    private async Task SearchAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            return;
        }

        await _navigationService.NavigateToPlayerAsync(SearchText);

        SearchText = null;
    }
}