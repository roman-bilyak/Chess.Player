using AndroidX.Core.Content;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class AndroidShellToolbarAppearanceTracker(IShellContext shellContext) : ShellToolbarAppearanceTracker(shellContext)
{
    public override void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
    {
        base.SetAppearance(toolbar, toolbarTracker, appearance);

        if (Platform.CurrentActivity is null)
        {
            return;
        }

        toolbar.OverflowIcon?.SetTint(ContextCompat.GetColor(Platform.CurrentActivity, Resource.Color.m3_ref_palette_white));
    }
}