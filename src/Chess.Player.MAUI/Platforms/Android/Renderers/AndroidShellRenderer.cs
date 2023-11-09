using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class AndroidShellRenderer : ShellRenderer
{
    public AndroidShellRenderer(Context context) : base(context)
    {
    }
    protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
    {
        return new AndroidShellItemRenderer(this);
    }

    protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
    {
        return new AndroidShellToolbarAppearanceTracker(this);
    }
}