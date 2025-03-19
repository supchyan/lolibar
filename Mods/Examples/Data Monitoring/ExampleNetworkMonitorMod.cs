using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleNetworkMonitorMod : LolibarMod
{
    LolibarContainer? NetworkMonitorContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        NetworkMonitorContainer = new()
        {
            Name = "ExampleNetworkMonitorContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetNetworkInfo(),
            Icon = LolibarDefaults.GetNetworkIcon(),
            MouseRightButtonUpEvent = SwapNetworkInfoEvent
        };
        NetworkMonitorContainer.Create();
    }
    public override void Update()
    {
        NetworkMonitorContainer.Text = LolibarDefaults.GetNetworkInfo();
        NetworkMonitorContainer.Icon = LolibarDefaults.GetNetworkIcon();
        NetworkMonitorContainer.Update();
    }

    void SwapNetworkInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapNetworkInfo();
    }
}

