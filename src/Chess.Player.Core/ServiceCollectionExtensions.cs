using Chess.Player.Cache;
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
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()

            .AddTransient<ICacheManager, CacheManager>()
            .AddTransient<ICache<PlayerGroupInfo>, FileCache<PlayerGroupInfo>>()
            .AddTransient<ICache<PlayerFullInfo>, FileCache<PlayerFullInfo>>()
            .AddTransient<ICache<TournamentInfo>, FileCache<TournamentInfo>>()
            .AddTransient<ICache<PlayerInfo>, FileCache<PlayerInfo>>();
    }
}