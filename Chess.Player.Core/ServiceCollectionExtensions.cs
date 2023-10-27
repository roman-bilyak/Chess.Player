using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Chess.Player;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChessServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IChessDataService, ChessDataService>()
            .AddTransient<IPlayerGroupService, PlayerGroupService>()
            .AddTransient<IChessDataManager, ChessDataManager>()
            .AddTransient<IChessDataFetcher, ChessResultsDataFetcher>()
            .AddTransient<IChessDataNormalizer, ChessDataNormalizer>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>()
            .AddTransient<ICacheManager, NullCacheManager>();
    }
}