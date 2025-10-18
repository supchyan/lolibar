using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;
class ExampleToastMod : LolibarMod
{
    // Toasts are service message windows,
    // which shows information
    // for a short period of time.
    // Such feature exists in android apps for example.
    public override void PreInitialize() 
    {
        // Toast appears in the center of the screen below or upper lolibar,
        // depening on snap lolibar position.
        // Snapping can be modified like: 
        BarSnapToTop = true;

        // So lolibar will be pinned to top of the screen now.
    }
    public override void Initialize()
    {
        // Create clickable container:
        LolibarContainer container = new()
        {
            Text                = "Click me!",
            HasBackground       = true,
            Parent              = Lolibar.BarCenterContainer,
            MouseLeftButtonUp   = ShowToast // <- set left click event
        };
        container.Create();
    }
    public override void Update() { }
    private int ShowToast(MouseButtonEventArgs args)
    {
        // Create toast object:
        LolibarToast toast = new()
        {
            Text            = "Toast message", // Message to draw
            ShowTime        = 1000, // How long toast will be visible
            TextColor       = LolibarColor.FromHEX("#ff0000"), // Make text red
            BackgroundColor = LolibarColor.FromHEX("#ffffff"), // Make bg white,
            FontSize        = 24,
        };

        toast.Create();

        return 0;
    }
}