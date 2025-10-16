using LolibarApp.Source;
using LolibarApp.Source.Tools;

// TESTING FEATURES, DON'T TRY TO IMPLEMENT IT IN YOUR MOD
//namespace LolibarApp.Mods;

class StressMonitoringMod : LolibarMod
{
    LolibarContainer CPU_Container = new() 
    {
        Parent = Lolibar.BarCenterContainer,
        SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
    };
    LolibarContainer RAM_Container = new()
    {
        Parent = Lolibar.BarCenterContainer,
        SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
    };
    LolibarContainer DISK_Container = new()
    {
        Parent = Lolibar.BarCenterContainer,
        SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
    };
    LolibarContainer NETWORK_Container = new()
    {
        Parent = Lolibar.BarCenterContainer,
        SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
    };
    LolibarContainer CPROC_Container = new()
    {
        Parent = Lolibar.BarCenterContainer,
    };


    public override void PreInitialize() { }
    public override void Initialize()
    {
        CPU_Container.Create();
        RAM_Container.Create();
        DISK_Container.Create();
        NETWORK_Container.Create();
        CPROC_Container.Create();
    }
    public override void Update()
    {
        //(BarWidth, BarLeft) = LolibarHelper.OffsetLolibarToCenter(BarWidth, BarMargin);

        CPU_Container.Text = LolibarStats.CpuTotalInPercent;
        RAM_Container.Text = LolibarStats.RamUsedInPercent;
        DISK_Container.Text = LolibarStats.DiskTotalInPercent;
        NETWORK_Container.Text = LolibarStats.NetworkBytesTotal;
        CPROC_Container.Text = LolibarStats.CurrentApplicationName;

        CPU_Container.Update();
        RAM_Container.Update();
        DISK_Container.Update();
        NETWORK_Container.Update();
        CPROC_Container.Update();
    }
}