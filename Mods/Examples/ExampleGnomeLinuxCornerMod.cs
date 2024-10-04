﻿using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleGnomeLinuxCornerMod : LolibarMod
{
    public override void PreInitialize()
    {
        // Set it to true, to enable the feature:
        BarCornersInvokesDesktopsMenu = true;

        // Now you can simulate WIN+TAB key press,
        // quick moving your mouse cursor to the Left corner of the statusbar.

        // You can modify the target corner:
        BarTargetCorner = LolibarEnums.BarTargetCorner.Right;

        // This property equals Left by default, so let's get back to it:
        BarTargetCorner = LolibarEnums.BarTargetCorner.Left;

        // So now, this feature compared to the Left corner of your screen!

        // But wait, which corner? Top Left? Bottom Left? Both???

        // 😎

        // Depends on which side of the screen your statusbar had been snapped.

        // You can modify snapping with `BarSnapToTop` property, like:
        BarSnapToTop = false;

        // Now statusbar will be drawn at the BOTTOM of the screen,
        // so the Left corner now is the Bottom Left corner.

        // Let's snap it back to top:
        BarSnapToTop = true;

        // Ok, statusbar at the TOP of the screen,
        // so the Left corner is the Top Left corner for now.

        // That's it!
    }
    public override void Initialize() { }
    public override void Update() { }
}
