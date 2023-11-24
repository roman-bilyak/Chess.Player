using Chess.Player.Data;
using Chess.Player.MAUI.Features.Players;
using Chess.Player.MAUI.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.Features.Home;

public partial class HomeViewModel : BaseRefreshViewModel
{
    private readonly IHistoryService _historyService;
    private readonly INavigationService _navigationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private ObservableCollection<PlayerShortViewModel> _players = [];

    public HomeViewModel
    (
        IHistoryService playerHistoryService,
        INavigationService navigationService,
        IDateTimeProvider dateTimeProvider,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(navigationService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _historyService = playerHistoryService;
        _navigationService = navigationService;
        _dateTimeProvider = dateTimeProvider;
        _serviceProvider = serviceProvider;
    }

    protected override async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        List<PlayerShortViewModel> players = [];
        DateTime currentDate = _dateTimeProvider.UtcNow.Date;
        foreach (PlayerFullInfo playerInfo in await _historyService.GetAllAsync(UseCache, cancellationToken))
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