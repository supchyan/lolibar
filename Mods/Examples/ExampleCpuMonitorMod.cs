using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleCpuMonitorMod : LolibarMod
{
    LolibarContainer? CpuMonitorContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        CpuMonitorContainer = new()
        {
            Name = "ExampleCpuMonitorContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetCpuInfo(),
            Icon = LolibarDefaults.GetCpuIcon()
        };
        CpuMonitorContainer.Create();
    }
    public override void Update()
    {
        CpuMonitorContainer.Text = LolibarDefaults.GetCpuInfo();
        CpuMonitorContainer.Icon = LolibarDefaults.GetCpuIcon();
        CpuMonitorContainer.Update();
    }
}

