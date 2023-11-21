using Chess.Player.Cache;
using System.Diagnostics.CodeAnalysis;

namespace Chess.Player.MAUI.Services;

internal class SettingsService : ISettingsService
{
    private SettingsInfo? _settingsInfo;

    private readonly ICacheManager _cacheManager;

    public SettingsService
    (
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(cacheManager);

        _cacheManager = cacheManager;
    }

    public event ThemeChangedEventHandler? ThemeChanged;

    public async Task<AppTheme> GetThemeAsync(CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        return _settingsInfo.Theme;
    }

    public async Task SetThemeAsync(AppTheme theme, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        if (_settingsInfo.Theme != theme)
        {
            _settingsInfo.Theme = theme;

            await SaveAsync(cancellationToken);

            ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(_settingsInfo.Theme));
        }
    }

    #region helper methods

    [MemberNotNull(nameof(_settingsInfo))]
    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _settingsInfo ??= await _cacheManager.GetAsync<SettingsInfo>(includeExpired: false, cancellationToken) ?? new SettingsInfo { Theme = AppTheme.Unspecified };
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.AddAsync(_settingsInfo, expirationDate: null, cancellationToken);
    }

    #endregion
}
