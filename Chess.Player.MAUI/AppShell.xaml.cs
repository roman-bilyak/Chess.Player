namespace Chess.Player.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.ShellSectionChanged)
            {
                var navigation = Shell.Current.Navigation;
                var pages = navigation.NavigationStack;
                for (var i = pages.Count - 1; i >= 1; i--)
                {
                    navigation.RemovePage(pages[i]);
                }
            }
        }
    }
}