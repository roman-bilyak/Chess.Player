using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class IOSShellRenderer : ShellRenderer
{
    protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
    {
        var renderer = base.CreateShellSectionRenderer(shellSection);
        if (renderer != null)
        {
            var tabbarController = (renderer as ShellSectionRenderer).TabBarController;
            if (null != tabbarController)
            {
                tabbarController.ViewControllerSelected += async (o, e) => {
                    await shellSection?.Navigation.PopToRootAsync();

                };
            }
        }
        return renderer;
    }
}