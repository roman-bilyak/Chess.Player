using Chess.Player;
using Chess.Player.Data;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddChessServices()
            .BuildServiceProvider();
try
{
    List<string> names = new()
    {
        "Мосесов Даниїл",
        "Мосесов Даниил",
        "Мосесов Данило",
        "Mosesov Danyil",
        "Mosesov Danylo",
    };
    string groupName = names.First();

    IPlayerGroupService playerGroupService = serviceProvider.GetRequiredService<IPlayerGroupService>();
    foreach (string name in names)
    {
        await playerGroupService.AddToGroupAsync(groupName, name, CancellationToken.None);
    }

    ConsoleOutput consoleOutput = new();
    IChessDataService chessDataService = serviceProvider.GetRequiredService<IChessDataService>();
    chessDataService.ProgressChanged += (sender, args) => consoleOutput.DisplayProgress(args.ProgressPercentage);

    PlayerFullInfo playerFullInfo = await chessDataService.GetPlayerFullInfoAsync(groupName, true, CancellationToken.None);
    consoleOutput.DisplayPlayerFullInfo(playerFullInfo);
}
finally
{
    await serviceProvider.DisposeAsync();
}

Console.ReadLine();