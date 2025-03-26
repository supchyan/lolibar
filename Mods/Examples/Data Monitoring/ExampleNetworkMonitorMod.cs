using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

// namespace LolibarApp.Mods;

class ExampleNetworkMonitorMod : LolibarMod
{
    LolibarContainer NetworkMonitorContainer = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        NetworkMonitorContainer = new()
        {
            Name = "ExampleNetworkMonitorContainer",
            Parent = Lolibar.BarLeftContainer,
            MouseRightButtonUp = SwapNetworkInfo,
        };
        NetworkMonitorContainer.Create();
    }
    public override void Update()
    {
        NetworkMonitorContainer.Text = LolibarDefaults.GetNetworkInfo();
        NetworkMonitorContainer.Icon = LolibarDefaults.GetNetworkIcon();
        NetworkMonitorContainer.Update();
    }
    int SwapNetworkInfo(MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapNetworkInfo();
        return 0;
    }
}

