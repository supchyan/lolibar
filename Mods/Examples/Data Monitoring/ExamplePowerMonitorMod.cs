using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExamplePowerMonitorMod : LolibarMod
{
    LolibarContainer PowerMonitorContainer = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        PowerMonitorContainer   = new()
        {
            Parent              = Lolibar.BarRightContainer,
            MouseLeftButtonUp   = OpenPowerSettings
        };
        PowerMonitorContainer.Create();
    }
    public override void Update()
    {
        // Power info in percent:
        PowerMonitorContainer.Text = LolibarStats.PowerInPercent;

        // This "smart" icon changes upon battery state swap (critical / low / charging / etc.).
        PowerMonitorContainer.Icon = LolibarStats.SmartPowerIcon;

        PowerMonitorContainer.Update();
    }
    // Open Windows power settings event just for fun:
    int OpenPowerSettings(MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:batterysaver",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();

        return 0;
    }
}