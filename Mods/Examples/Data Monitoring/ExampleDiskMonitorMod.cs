using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

// namespace LolibarApp.Mods;

class ExampleDiskMonitorMod : LolibarMod
{
    LolibarContainer DiskMonitorContainer = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        DiskMonitorContainer    = new()
        {
            Name                = "ExampleDiskMonitorContainer",
            Parent              = Lolibar.BarLeftContainer,
            MouseRightButtonUp  = SwapDiskInfo,
        };
        DiskMonitorContainer.Create();
    }
    public override void Update()
    {
        DiskMonitorContainer.Text = LolibarDefaults.GetDiskInfo();
        DiskMonitorContainer.Icon = LolibarDefaults.GetDiskIcon();
        DiskMonitorContainer.Update();
    }
    int SwapDiskInfo(MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapDiskInfo(); 
        return 0;
    }
}