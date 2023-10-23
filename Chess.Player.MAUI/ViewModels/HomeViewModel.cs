using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class HomeViewModel : BaseViewModel
{
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    public HomeViewModel
    (
        IPlayerHistoryService playerHistoryService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _playerHistoryService = playerHistoryService;
        _navigationService = navigationService;

        PlayerCardList = new(_navigationService);
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        PlayerCardList.Players.Clear();

        foreach (var player in await _playerHistoryService.GetAllAsync(cancellationToken))
        {
            PlayerCardList.Players.Add(new PlayerCardViewModel { LastName = player });
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await _navigationService.PushAsync<PlayerPage, PlayerViewModel>(x =>
        {
            x.SearchCriterias.Add(SearchText);
        });
    }
}