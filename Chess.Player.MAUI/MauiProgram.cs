using Chess.Player.Data;
using Chess.Player.MAUI.ViewModels;
using Chess.Player.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

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
            builder.Services.AddTransient<IPopupService, MAUIPopupService>();

            builder.Services.AddTransient<IChessDataManager, ChessDataManager>();
            builder.Services.AddTransient<IChessDataService, ChessDataService>();
            builder.Services.AddTransient<IChessDataFetcher, ChessResultsDataFetcher>();
            builder.Services.AddTransient<ICacheManager, MAUIFileCacheManager>();
            builder.Services.AddTransient<IOutputFormatter, ConsoleOutputFormatter>();

            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddTransient<PlayerViewModel>();

            return builder.Build();
        }
    }
}