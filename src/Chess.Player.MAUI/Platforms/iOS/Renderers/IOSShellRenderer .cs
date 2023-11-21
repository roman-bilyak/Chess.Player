using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class IOSShellRenderer : ShellRenderer
{
    protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
    {
        var renderer = base.CreateShellSectionRenderer(shellSection);

        var tabbarController = (renderer as ShellSectionRenderer)?.TabBarController;
        if (tabbarController is not null)
        {
            tabbarController.ViewControllerSelected += async (o, e) => {
                await shellSection.Navigation.PopToRootAsync();
            };
        }

        return renderer;
    }
}