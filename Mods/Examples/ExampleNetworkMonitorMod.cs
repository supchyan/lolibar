using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod outside of mods namespace, so won't be loaded
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

