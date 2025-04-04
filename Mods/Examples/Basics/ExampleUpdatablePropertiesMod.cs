﻿using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleUpdatablePropertiesMod : LolibarMod
{
    // How to resize lolibar when the screen resolution has changed?
    //
    // Lolibar lib has a Vector2 called `Lolibar.Inch_Screen`,
    // that store current screen resolution in Inches.
    // Feel free to use it!
    public override void PreInitialize() { }
    public override void Initialize() { }
    public override void Update()
    {
        // Update Width property, depending on screen size and current margin property:
        BarWidth = Lolibar.Inch_Screen.X - 2 * BarMargin;

        // Then change statusbar position depends on current Width.
        // In this example, I center it horizontally:
        BarLeft = (Lolibar.Inch_Screen.X - BarWidth) / 2;

        // Alternatively, you can simplify calculations above with this line:
        (BarWidth, BarLeft) = LolibarHelper.OffsetLolibarToCenter(BarWidth, BarMargin);
    }
}
