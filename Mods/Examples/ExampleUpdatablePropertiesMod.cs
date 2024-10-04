using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleUpdatablePropertiesMod : LolibarMod
{
    // How to resize lolibar when screen resolution has changed?
    //
    // Lolibar libraries has `Lolibar.Inch_Screen` Vector2,
    // which stores current screen resolution in Inches.
    // Feel free to use it!
    public override void PreInitialize() { }
    public override void Initialize() { }
    public override void Update()
    {
        // Update Width property:
        BarWidth = Lolibar.Inch_Screen.X - 2 * BarMargin;

        // Then change statusbar position depends on current Width.
        // In this example, I center it horizontally:
        BarLeft = (Lolibar.Inch_Screen.X - BarWidth) / 2;
    }
}