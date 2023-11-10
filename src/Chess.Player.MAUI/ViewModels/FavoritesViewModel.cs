using Chess.Player.Data;
using Chess.Player.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IServiceProvider _serviceProvider;

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

    public FavoritesViewModel
    (
        IPlayerFavoriteService playerFavoriteService,
        IServiceProvider serviceProvider
    )
    {
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _playerFavoriteService = playerFavoriteService;
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

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            PlayerList.Players.Clear();

            foreach (PlayerFullInfo player in await _playerFavoriteService.GetAllAsync(UseCache, cancellationToken))
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
}