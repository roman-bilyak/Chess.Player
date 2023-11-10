using Chess.Player.MAUI.Services;

namespace Chess.Player.MAUI
{
    public partial class App : Application
    {
        public App
        (
            IServiceProvider serviceProvider
        )
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            ISettingsService settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            settingsService.ThemeChanged += (sender, args) => { UserAppTheme = args.Theme; };

            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }
    }
}