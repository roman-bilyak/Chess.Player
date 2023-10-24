using Chess.Player;
using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddChessServices()
            .AddTransient<ICacheManager, ConsoleFileCacheManager>()
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

    ConsoleOutput consoleOutput = new();
    IChessDataService chessDataService = serviceProvider.GetRequiredService<IChessDataService>();
    chessDataService.ProgressChanged += (sender, args) => consoleOutput.DisplayProgress(args.ProgressPercentage);

    SearchResult searchResult = await chessDataService.SearchAsync(searchCriterias, true, CancellationToken.None);
    consoleOutput.DisplayResult(searchResult);
}
finally
{
    await serviceProvider.DisposeAsync();
}

Console.ReadLine();