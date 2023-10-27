using Chess.Player.Data;
using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class HomeViewModel : BaseViewModel
{
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private bool _forceRefresh;

    public HomeViewModel
    (
        IPlayerHistoryService playerHistoryService,
        INavigationService navigationService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(navigationService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _playerHistoryService = playerHistoryService;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;

        PlayerCardList = _serviceProvider.GetRequiredService<PlayerCardListViewModel>();
    }

    [RelayCommand]
    private Task StartAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;
        ForceRefresh = false;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            PlayerCardList.Players.Clear();

            foreach (PlayerFullInfo player in await _playerHistoryService.GetAllAsync(ForceRefresh, cancellationToken))
            {
                PlayerCardViewModel playerCardViewModel = _serviceProvider.GetRequiredService<PlayerCardViewModel>();

                playerCardViewModel.Names = new ObservableCollection<NameViewModel>(player.Names.Select(x => new NameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
                playerCardViewModel.Title = player.Title;
                playerCardViewModel.ClubCity = player.ClubCity;
                playerCardViewModel.YearOfBirth = player.YearOfBirth;

                PlayerCardList.Players.Add(playerCardViewModel);
            }
        }
        finally
        {
            IsLoading = false;
            ForceRefresh = false;
        }
    }

    [RelayCommand]
    private async Task SearchAsync(CancellationToken cancellationToken)
    {
        await _navigationService.PushAsync<PlayerPage, PlayerViewModel>(x =>
        {
            x.Name = SearchText;
        });

        SearchText = null;
    }
}