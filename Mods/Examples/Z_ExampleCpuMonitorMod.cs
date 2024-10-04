using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod outside of mods namespace, so won't be loaded
//namespace LolibarApp.Mods;

class Z_ExampleCpuMonitorMod : LolibarMod
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

