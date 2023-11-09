using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class AndroidShellItemRenderer : ShellItemRenderer
{
    public AndroidShellItemRenderer(IShellContext shellContext) : base(shellContext)
    {
    }

    protected override void OnTabReselected(ShellSection shellSection)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await shellSection?.Navigation.PopToRootAsync();
        });
    }

    protected override bool ChangeSection(ShellSection shellSection)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await shellSection?.Navigation.PopToRootAsync();
        });
        return base.ChangeSection(shellSection);
    }
}