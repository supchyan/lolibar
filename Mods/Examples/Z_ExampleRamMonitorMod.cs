using LolibarApp.Source;
using LolibarApp.Source.Tools;

//namespace LolibarApp.Mods;

class Z_ExampleRamMonitorMod : LolibarMod
{
    LolibarContainer? RamMonitorContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        RamMonitorContainer = new()
        {
            Name = "ExampleRamMonitorContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetRamInfo(),
            Icon = LolibarDefaults.GetRamIcon(),
            MouseRightButtonUpEvent = SwapRamInfoEvent
        };
        RamMonitorContainer.Create();
    }
    public override void Update()
    {
        RamMonitorContainer.Text = LolibarDefaults.GetRamInfo();
        RamMonitorContainer.Icon = LolibarDefaults.GetRamIcon();
        RamMonitorContainer.Update();
    }

    void SwapRamInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapRamInfo();
    }
}

