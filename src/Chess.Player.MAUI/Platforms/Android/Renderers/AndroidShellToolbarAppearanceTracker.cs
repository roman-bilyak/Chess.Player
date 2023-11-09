using AndroidX.Core.Content;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chess.Player.MAUI;

internal class AndroidShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
{
    public AndroidShellToolbarAppearanceTracker(IShellContext shellContext)
        : base(shellContext)
    {
    }

    public override void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
    {
        base.SetAppearance(toolbar, toolbarTracker, appearance);

        toolbar.OverflowIcon.SetTint(ContextCompat.GetColor(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, Resource.Color.m3_ref_palette_white));
    }
}