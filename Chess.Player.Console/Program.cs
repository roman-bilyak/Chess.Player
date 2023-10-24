using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IChessDataService, ChessDataService>()
            .AddTransient<IChessDataManager, ChessDataManager>()
            .AddTransient<IChessDataFetcher, ChessResultsDataFetcher>()
            .AddTransient<ICacheManager, ConsoleFileCacheManager>()
            .AddTransient<IOutputFormatter, ConsoleOutputFormatter>()
            .BuildServiceProvider();
try
{
    SearchCriteria[] searchCriterias = new SearchCriteria[]
    {
        new SearchCriteria("Мосесов Даниїл"),
        new SearchCriteria("Мосесов Даниил"),
        new SearchCriteria("Мосесов Данило"),
        new SearchCriteria("Mosesov Danyil"),
        new SearchCriteria("Mosesov Danylo"),
    };

    IChessDataService chessDataManager = serviceProvider.GetRequiredService<IChessDataService>();
    await chessDataManager.SearchAsync(searchCriterias, CancellationToken.None);
}
finally
{
    await serviceProvider.DisposeAsync();
}

Console.ReadLine();