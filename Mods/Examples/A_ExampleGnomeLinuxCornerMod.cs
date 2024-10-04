using LolibarApp.Source.Tools;

namespace LolibarApp.Mods;

class A_ExampleGnomeLinuxCornerMod : LolibarMod
{
    public override void PreInitialize()
    {
        // Set it to true, to enable feature:
        BarCornersInvokesDesktopsMenu = true;

        // Now you can simulate WIN+TAB key press,
        // quick moving your mouse cursor to left corner of the statusbar.

        // You can choose target corner:
        BarTargetCorner = LolibarEnums.BarTargetCorner.Right;

        // This value is default, so let's get back to it:
        BarTargetCorner = LolibarEnums.BarTargetCorner.Left;

        // So now, this feature compared to Left corner of the screen!

        // But wait, which corner? Top Left? Bottom Left? Both???

        // 😎

        // Depends on which side of the screen your statusbar has been snapped.

        // You can modify snapping with `BarSnapToTop` property, like:
        BarSnapToTop = false;

        // Now statusbar will be drawn at the BOTTOM of the screen,
        // so Left corner now is Bottom Left corner.

        // Let's snap it back to top:
        BarSnapToTop = true;

        // Ok, statusbar at the TOP of the screen,
        // so Left corner is Top Left corner for now.

        // That's it!
    }
    public override void Initialize() { }
    public override void Update() { }
}
