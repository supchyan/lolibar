using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;
class ExampleFontMod : LolibarMod
{
    public override void PreInitialize()
    {
        // To use custom font you need to do this:
        // 1. Get any font you want and install it globally in windows.
        // 2. Move font file (.ttf) inside Lolibar Fonts folder
        // 3. Set `BarFontFamily` as font name:
        BarFontFamily   = "mononoki";
        // You can specify font size:
        BarFontSize     = 16;
        // And weight:
        BarFontWeight   = 600;
        
        // You can do this locally for each container,
        // so multifont statusbar is real.
    }
    public override void Initialize()
    {
        LolibarContainer fontContainer = new()
        {
            // Set text to draw:
            Text = "mama mia!!!",

            // You can find this font in lolibar's Fonts folder
            FontFamily = "Monsieur La Doulaise", // <- this is font name, not a file name!
            FontSize = 32,
            FontWeight = 400,

            // Set parent to spawn it in there:
            Parent = Lolibar.BarCenterContainer
        };
        fontContainer.Create();
    } 
    public override void Update()
    {
        
    }
}