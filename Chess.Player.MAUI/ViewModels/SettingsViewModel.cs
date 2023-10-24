using Chess.Player.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class SettingsViewModel : BaseViewModel
{
    private readonly ICacheManager _cacheManager;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    public SettingsViewModel
    (
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(cacheManager);

        _cacheManager = cacheManager;
    }

    [RelayCommand]
    private async Task ClearCacheAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.DeleteAsync(cancellationToken);
    }
}