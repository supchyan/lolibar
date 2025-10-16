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
            Parent              = Lolibar.BarLeftContainer,
        };
        DiskMonitorContainer.Create();
    }
    public override void Update()
    {
        // Total disk info:
        DiskMonitorContainer.Text = LolibarStats.DiskTotalInPercent;
        DiskMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/disk.svg");

        // You can get read / write info as well:
        // DiskMonitorContainer.Text = LolibarDefaults.DiskReadInPercent;
        // DiskMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/disk_read.svg");

        // DiskMonitorContainer.Text = LolibarDefaults.DiskWriteInPercent;
        // DiskMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/disk_write.svg");

        DiskMonitorContainer.Update();
    }
}