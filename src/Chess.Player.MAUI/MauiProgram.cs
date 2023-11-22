using Chess.Player.Cache;
using Chess.Player.Data;
using Chess.Player.MAUI.Features;
using Chess.Player.MAUI.Features.Favorites;
using Chess.Player.MAUI.Features.Home;
using Chess.Player.MAUI.Features.Info;
using Chess.Player.MAUI.Features.Players;
using Chess.Player.MAUI.Features.PlayerTournaments;
using Chess.Player.MAUI.Features.Settings;
using Chess.Player.MAUI.Features.Tournaments;
using CommunityToolkit.Maui;

#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace Chess.Player.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMauiHandlers((handlers) =>
            {
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(Chess.Player.MAUI.AndroidShellRenderer));
#elif IOS
                handlers.AddHandler(typeof(Shell), typeof(Chess.Player.MAUI.IOSShellRenderer));
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddChessServices();

        builder.Services.AddTransient<ICache<SettingsInfo>, AppDataFileCache<SettingsInfo>>();
        builder.Services.AddTransient<ICache<FavoriteList>, AppDataFileCache<FavoriteList>>();
        builder.Services.AddTransient<ICache<HistoryList>, AppDataFileCache<HistoryList>>();
        builder.Services.AddTransient<ICache<PlayerGroupInfo>, AppDataFileCache<PlayerGroupInfo>>();

        builder.Services.AddTransient<ICache<PlayerFullInfo>, CacheDataFileCache<PlayerFullInfo>>();
        builder.Services.AddTransient<ICache<PlayerTournamentList>, CacheDataFileCache<PlayerTournamentList>>();
        builder.Services.AddTransient<ICache<TournamentInfo>, CacheDataFileCache<TournamentInfo>>();
        builder.Services.AddTransient<ICache<PlayerInfo>, CacheDataFileCache<PlayerInfo>>();

        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IHistoryService, HistoryService>();
        builder.Services.AddSingleton<IFavoriteService, FavoriteService>();
        builder.Services.AddTransient<INavigationService, MAUINavigationService>();
        builder.Services.AddTransient<IPopupService, MAUIPopupService>();

        builder.Services.AddTransient<AppShellViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<PlayerViewModel>();
        builder.Services.AddTransient<PlayerTournamentViewModel>();
        builder.Services.AddTransient<TournamentViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<CacheViewModel>();
        builder.Services.AddTransient<InfoViewModel>();

        builder.Services.AddTransient<PlayerShortViewModel>();
        builder.Services.AddTransient<PlayerTournamentShortViewModel>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<PlayerScoreViewModel>();

        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<HomeView>();
        builder.Services.AddTransient<PlayerView>();
        builder.Services.AddTransient<PlayerTournamentView>();
        builder.Services.AddTransient<TournamentView>();
        builder.Services.AddTransient<FavoritesView>();
        builder.Services.AddTransient<SettingsView>();
        builder.Services.AddTransient<CacheView>();
        builder.Services.AddTransient<InfoView>();

        return builder.Build();
    }
}