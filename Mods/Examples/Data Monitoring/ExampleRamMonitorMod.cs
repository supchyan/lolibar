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
            MouseRightButtonUp  = SwapRamInfo,
        };
        RamMonitorContainer.Create();
    }
    public override void Update()
    {
        RamMonitorContainer.Text = LolibarDefaults.GetRamInfo();
        RamMonitorContainer.Icon = LolibarDefaults.GetRamIcon();
        RamMonitorContainer.Update();
    }
    int SwapRamInfo(MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapRamInfo();
        return 0;
    }
}

