using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IChessDataManager, ChessDataManager>()
            .AddTransient<IChessDataService, ChessDataService>()
            .AddTransient<IChessDataFetcher, ChessResultsDataFetcher>()
            .AddTransient<ICacheManager, ConsoleFileCacheManager>()
            .AddTransient<IOutputFormatter, ConsoleOutputFormatter>()
            .BuildServiceProvider();
try
{
    SearchCriteria[] searchCriterias = new SearchCriteria[]
    {
        new SearchCriteria("Мосесов", "Даниїл"),
        new SearchCriteria("Мосесов", "Даниил"),
        new SearchCriteria("Мосесов", "Данило"),
        new SearchCriteria("Mosesov", "Danyil"),
        new SearchCriteria("Mosesov", "Danylo"),
    };

    IChessDataManager chessDataManager = serviceProvider.GetRequiredService<IChessDataManager>();
    await chessDataManager.SearchAsync(searchCriterias, CancellationToken.None);
}
finally
{
    await serviceProvider.DisposeAsync();
}

Console.ReadLine();