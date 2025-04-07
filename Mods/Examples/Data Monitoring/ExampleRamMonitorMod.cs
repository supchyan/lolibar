using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

// namespace LolibarApp.Mods;

class ExampleRamMonitorMod : LolibarMod
{
    LolibarContainer RamMonitorContainer = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        RamMonitorContainer     = new()
        {
            Name                = "ExampleRamMonitorContainer",
            Parent              = Lolibar.BarLeftContainer,
        };
        RamMonitorContainer.Create();
    }
    public override void Update()
    {
        // Total RAM usage in percent:
        RamMonitorContainer.Text = LolibarStats.RamUsedInPercent;

        // Total RAM usage in Gigabytes:
        RamMonitorContainer.Text = LolibarStats.RamUsedInGigabytes;


        RamMonitorContainer.Update();
    }
}

