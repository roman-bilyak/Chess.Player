﻿using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IChessDataManager, ChessDataManager>()
            .AddTransient<IChessDataService, ChessDataService>()
            .AddTransient<IChessDataFetcher, ChessResultsDataFetcher>()
            .AddTransient<ICacheManager, FileCacheManager>()
            .AddTransient<IOutputFormatter, ConsoleOutputFormatter>()
            .BuildServiceProvider();
try
{
    SearchCriteria[] searchCriterias = new SearchCriteria[]
    {
        new SearchCriteria("Мосесов", "Даниїл"),
        new SearchCriteria("Мосесов", "Даниил"),
        new SearchCriteria("Mosesov", "Danyil"),
    };

    IChessDataManager chessDataManager = serviceProvider.GetRequiredService<IChessDataManager>();
    await chessDataManager.SearchAsync(searchCriterias);
}
finally
{
    await serviceProvider.DisposeAsync();
}

Console.ReadLine();