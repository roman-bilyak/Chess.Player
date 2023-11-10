using Chess.Player.Data;
using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

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
    private PlayerListViewModel _playerList;

    [ObservableProperty]
    private bool _useCache;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _error;

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

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

        PlayerList = _serviceProvider.GetRequiredService<PlayerListViewModel>();
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
            PlayerList.Players.Clear();

            foreach (PlayerFullInfo player in await _playerHistoryService.GetAllAsync(UseCache, cancellationToken))
            {
                PlayerViewModel playerCardViewModel = _serviceProvider.GetRequiredService<PlayerViewModel>();

                playerCardViewModel.Names = new ObservableCollection<NameViewModel>(player.Names.Select(x => new NameViewModel { LastName = x.LastName, FirstName = x.FirstName }));
                playerCardViewModel.Title = player.Title;
                playerCardViewModel.ClubCity = player.ClubCity;
                playerCardViewModel.YearOfBirth = player.YearOfBirth;

                PlayerList.Players.Add(playerCardViewModel);
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
        await _navigationService.PushAsync<PlayerFullPage, PlayerFullViewModel>(x =>
        {
            x.Name = SearchText;
        });

        SearchText = null;
    }
}