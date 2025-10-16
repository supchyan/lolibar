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
            Parent = Lolibar.BarLeftContainer,
        };
        NetworkMonitorContainer.Create();
    }
    public override void Update()
    {
        // Total network usage info:
        NetworkMonitorContainer.Text = LolibarStats.NetworkBytesTotal;
        NetworkMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/network.svg");

        // And of course, you can get separated info in a way like:

        // NetworkMonitorContainer.Text = LolibarDefaults.NetworkBytesSent;
        // NetworkMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/network_sent.svg");

        // NetworkMonitorContainer.Text = LolibarDefaults.NetworkBytesReceived;
        // NetworkMonitorContainer.Icon = LolibarIcon.ParseSVG("./Defaults/network_received.svg");

        NetworkMonitorContainer.Update();
    }
}

