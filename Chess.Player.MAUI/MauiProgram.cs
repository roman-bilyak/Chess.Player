using Chess.Player.Data;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddChessServices();
            builder.Services.AddTransient<ICacheManager, MAUIFileCacheManager>();

            builder.Services.AddSingleton<IPlayerHistoryService, PlayerHistoryService>();
            builder.Services.AddSingleton<IPlayerFavoriteService, PlayerFavoriteService>();
            builder.Services.AddTransient<INavigationService, MAUINavigationService>();
            builder.Services.AddTransient<IPopupService, MAUIPopupService>();

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<PlayerViewModel>();
            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<PlayerPage>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<SettingsPage>();

            return builder.Build();
        }
    }
}