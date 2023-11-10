using Chess.Player.Cache;
using Chess.Player.Data;
using Chess.Player.MAUI.Cache;
using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.MAUI.ViewModels;
using Chess.Player.Services;
using CommunityToolkit.Maui;

#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace Chess.Player.MAUI
{
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
            builder.Services.AddTransient<ICache<PlayerFavoriteList>, AppDataFileCache<PlayerFavoriteList>>();
            builder.Services.AddTransient<ICache<PlayerHistoryList>, AppDataFileCache<PlayerHistoryList>>();
            builder.Services.AddTransient<ICache<PlayerGroupInfo>, AppDataFileCache<PlayerGroupInfo>>();

            builder.Services.AddTransient<ICache<PlayerFullInfo>, CacheDataFileCache<PlayerFullInfo>>();
            builder.Services.AddTransient<ICache<TournamentInfo>, CacheDataFileCache<TournamentInfo>>();
            builder.Services.AddTransient<ICache<PlayerInfo>, CacheDataFileCache<PlayerInfo>>();

            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<IPlayerHistoryService, PlayerHistoryService>();
            builder.Services.AddSingleton<IPlayerFavoriteService, PlayerFavoriteService>();
            builder.Services.AddTransient<INavigationService, MAUINavigationService>();
            builder.Services.AddTransient<IPopupService, MAUIPopupService>();

            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<PlayerFullViewModel>();
            builder.Services.AddTransient<PlayerTournamentFullViewModel>();
            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<CacheViewModel>();

            builder.Services.AddTransient<PlayerListViewModel>();
            builder.Services.AddTransient<PlayerViewModel>();
            builder.Services.AddTransient<PlayerTournamentViewModel>();
            builder.Services.AddTransient<GameListViewModel>();
            builder.Services.AddTransient<GameViewModel>();

            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<PlayerFullPage>();
            builder.Services.AddTransient<PlayerTournamentFullPage>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<CachePage>();
            builder.Services.AddTransient<AboutPage>();

            return builder.Build();
        }
    }
}