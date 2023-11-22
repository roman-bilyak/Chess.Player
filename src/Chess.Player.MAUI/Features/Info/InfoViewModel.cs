using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.Features.Info;

[INotifyPropertyChanged]

public partial class InfoViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _version;

    public InfoViewModel()
    {
        Version = $"v{AppInfo.VersionString} ({AppInfo.BuildString})";
    }

    [RelayCommand]
    private static async Task BuyMeACoffeeAsync(CancellationToken cancellationToken)
    {
        await Launcher.OpenAsync(new Uri("https://www.buymeacoffee.com/chess.player"));
    }

    [RelayCommand]
    private static async Task SendEmailAsync(CancellationToken cancellationToken)
    {
        try
        {
            EmailMessage message = new()
            {
                Subject = "Support Inquiry",
                Body = "Hello, I have a question or need support regarding the Chess Player app.",
                To = ["chess.player@gmail.com"],
            };

            await Email.ComposeAsync(message);
        }
        catch
        {
        }
    }
}