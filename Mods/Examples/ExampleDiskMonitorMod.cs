using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleDiskMonitorMod : LolibarMod
{
    LolibarContainer? DiskMonitorContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        DiskMonitorContainer = new()
        {
            Name = "ExampleDiskMonitorContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetDiskInfo(),
            Icon = LolibarDefaults.GetDiskIcon(),
            MouseRightButtonUpEvent = SwapDiskInfoEvent
        };
        DiskMonitorContainer.Create();
    }
    public override void Update()
    {
        DiskMonitorContainer.Text = LolibarDefaults.GetDiskInfo();
        DiskMonitorContainer.Icon = LolibarDefaults.GetDiskIcon();
        DiskMonitorContainer.Update();
    }
    void SwapDiskInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapDiskInfo();
    }
}